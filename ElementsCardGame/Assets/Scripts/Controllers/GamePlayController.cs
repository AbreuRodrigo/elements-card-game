using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamePlayController : MonoBehaviour {
	public static GamePlayController instance;

	[Header("Configurations")]
	public GameMode gameMode;
	public GameState gameState;
	public bool useDefaultDeck = true;
	public bool hasSetupMixedCards;
	public int spellsInGame;

	[Header("Players")]
	public Player localPlayer;
	public Player opponentPlayer;
	private AIAgent aiAgent;

	[Header("Other Objects")]
	public Coin coin;

	[Header("Components")]
	public DiceManager diceManager;
	public SpellManager spellManager;
	public TurnManager turnManager;
	public ArcaneCircle bluePlayerArcaneCircle;//LocalPlayer
	public ArcaneCircle redPlayerArcaneCircle;//AI or Opponent when in multiplayer mode
	public MessageManager messageManager;

	void Awake() {
		if(instance == null) {
			instance = this;
		}

		iTween.Count ();
	}

	void Start() {
		ValidateGameMode ();

		ChangeGameToHeadsAndTailsState ();

		if (useDefaultDeck) {
			SetDefaultDeckToLocalPlayer ();
		}

		InitializeLocalPlayersDeck ();
		InitializeAIDeckOnHumanVSMachineGameMode ();

		GUIController.instance.interactionBlocker.Enable ();
		GUIController.instance.interactionBlocker.FadeIn ();
	}

	public void PressEndTurnButton() {
		SoundManager.instance.PlayClickSound ();

		GUIController.instance.HideInteractionBlocker ();

		GUIController.instance.HideEndTurnButton ();
	}

	public void PressDeckButtonLogics() {
		if(IsGamePlayState() && localPlayer.currentCard == null) {
			if(turnManager != null && turnManager.IsBeginningPhase()) {
				SoundManager.instance.PlayClickSound ();

				DrawTopCardFromDeck ();
			}
		}
	}

	//CARD'S SPELLCASTING BUTTONS
	public void PressCircleActionButton() {
		SoundManager.instance.PlayClickSound ();

		if(localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			//CastSpell (currentSelectedCard, SpellSelection.Circle);
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Circle);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressSquareActionButton() {
		SoundManager.instance.PlayClickSound ();

		if(localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			//CastSpell (currentSelectedCard, SpellSelection.Square);
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Square);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressRhombusActionButton() {
		SoundManager.instance.PlayClickSound ();

		if(localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			//CastSpell (currentSelectedCard, SpellSelection.Rhombus);
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Rhombus);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressSaveCardActionButton() {
		SoundManager.instance.PlayClickSound ();

		if (localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			GUIController.instance.HideCardActionButtons ();
			localPlayer.savedCard = localPlayer.currentCard;
			localPlayer.currentCard.PutToSavedState ();
		}
	}

	public void PressPutToMixActionButton() {
		SoundManager.instance.PlayClickSound ();

		if (localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateMixedSelection ()) {
			GUIController.instance.HideCardActionButtons ();
			localPlayer.currentCard.PutIntoWaitingToMixState ();
		}
	}

	public void ShowBluePlayerArcaneCircle() {
		if (bluePlayerArcaneCircle != null) {
			bluePlayerArcaneCircle.ShowArcaneCircle ();
		}
	}

	public void HideBluePlayerArcaneCircle() {
		if (bluePlayerArcaneCircle != null) {
			bluePlayerArcaneCircle.HideArcaneCircle ();
		}
	}

	public void ShowRedPlayerArcaneCircle() {
		if (redPlayerArcaneCircle != null) {
			redPlayerArcaneCircle.ShowArcaneCircle ();
			ProcessSpellsInGame ();
		}		
	}

	public void HideRedPlayerArcaneCircle() {
		if (redPlayerArcaneCircle != null) {
			redPlayerArcaneCircle.HideArcaneCircle ();
		}		
	}

	//Notificated by coin when whenever it's flipped
	public void NotificationFromCoinFlip(CoinResult result) {
		if(IsHeadsAndTailsState()) {
			if(localPlayer != null && localPlayer.stats != null) {
				localPlayer.stats.ShowPlayerStats ();
			}

			if(opponentPlayer != null && opponentPlayer.stats != null) {
				opponentPlayer.stats.ShowPlayerStats ();
			}

			ChangeGameStateByFirstCoinFlippingResult (result);
			
			GUIController.instance.ShowCardBases ();
		}

		if(result.Equals(CoinResult.Sword)) {
			coin.FadeOutFromSword ();
		}else if(result.Equals(CoinResult.Shield)) {
			coin.FadeOutFromShield ();
		}
	}

	public void NotificationFromDiceRoll(int diceFace) {
		
	}

	private void ProcessSpellsInGame() {
		spellsInGame++;

		if(spellsInGame == 2) {
			PassTurnPhase ();

			if(turnManager.IsCombatPhase()) {
				//opponentPlayer.currentCard.EnemyReveal ();
				//redPlayerArcaneCircle.HideArcaneCircle ();

				//localPlayer.currentCard.LocalPlayerReveal ();
				//bluePlayerArcaneCircle.HideArcaneCircle ();
			}
		}
	}

	public void PassTurnPhase() {
		if (turnManager != null) {
			turnManager.NextPhase ();				
		}
	}

	public void RollDice() {	
	}

	public void CastSpell(Card selectedCard, SpellSelection selection) {
		StartCoroutine (CallSpellCastingBySpellSelection (selectedCard, selection));
	}

	public void PutCardIntoGame() {
		PassTurnPhase ();

		if(IsHumanVSMachineGameMode()) {
			aiAgent.AIDrawTopCardFromDeck ();
		}

		ProcessSpellsInGame ();
	}

	private void DrawTopCardFromDeck() {
		if(IsGamePlayState()) {
			Card card = localPlayer.Deck.GetNextCard ();

			if (card != null) {
				card.gameObject.SetActive (true);
				card.MoveMeToCenter ();
				card.ChangeToInHandState ();
				localPlayer.currentCard = card;

				if (!localPlayer.Deck.HasNext ()) {
					GUIController.instance.DeckButtonTurnOff ();
				} 

				GUIController.instance.DecreaseRemainingCards ();
			}
		}
	}

	private void DrawMixedCardFromDeck() {
		if(IsMixedCardSetupState()) {
			localPlayer.mixedCard = localPlayer.Deck.GetMixedCardFromDeck ();
			localPlayer.currentCard = localPlayer.mixedCard;

			if(localPlayer.mixedCard != null) {
				localPlayer.mixedCard.gameObject.SetActive (true);
				localPlayer.mixedCard.MoveMeToCenter ();
				localPlayer.mixedCard.ChangeToMixedSelectionState ();

				GUIController.instance.DecreaseRemainingCards ();
				messageManager.ShowMixedCardMessage ();
			}
		}
		if(IsHumanVSMachineGameMode()) {
			aiAgent.AIDrawMixedCardFromDeck ();
		}
	}

	private void SetDefaultDeckToLocalPlayer() {
		DeckPatternManager.instance.BuildDefaultDeck1 (localPlayer);
	}

	private void InitializeLocalPlayersDeck() {
		if(localPlayer != null && localPlayer.Deck != null) {
			DeckManager.instance.ShufflePlayersCurrentDeck (localPlayer.Deck);
		}

		GUIController.instance.DefineRemainingCardsTxt (localPlayer.Deck.Size);
	}

	private void InitializeAIDeckOnHumanVSMachineGameMode() {
		if(IsHumanVSMachineGameMode() && opponentPlayer != null) {
			DeckPatternManager.instance.BuildDefaultDeck1 (opponentPlayer);

			if (opponentPlayer.Deck != null) {
				DeckManager.instance.ShufflePlayersCurrentDeck (opponentPlayer.Deck);
			}
		}
	}

	private void ValidateGameMode() {
		if(IsHumanVSMachineGameMode() && opponentPlayer != null && localPlayer != null) {
			opponentPlayer.gameObject.SetActive (true);
			opponentPlayer.playerName = "AI Player";
			opponentPlayer.HP = 80;
			opponentPlayer.localPlayer = false;
			opponentPlayer.isAI = true;
			aiAgent = opponentPlayer.GetComponent<AIAgent> ();
			aiAgent.enabled = true;
			aiAgent.aiPlayer = opponentPlayer;
			aiAgent.inGamePlayPoint = redPlayerArcaneCircle.transform.position;
		}
	}

	//GAME_MODE
	private bool IsHumanVSHumanGameMode() {
		return gameMode.Equals (GameMode.HumanVSHuman);
	}

	private bool IsHumanVSMachineGameMode() {
		return gameMode.Equals (GameMode.HumanVSMachine);
	}

	//GAME_STATE
	private bool IsHeadsAndTailsState() {
		return gameState.Equals (GameState.HeadsAndTailsState);
	}

	private bool IsMixedCardSetupState() {
		return gameState.Equals (GameState.MixedCardSetupState);
	}

	private bool IsGamePlayState() {
		return gameState.Equals (GameState.GamePlayState);
	}

	private bool IsGameOverState() {
		return gameState.Equals (GameState.GameOverState);
	}

	//State Transitions
	private void ChangeGameStateByFirstCoinFlippingResult(CoinResult result) {
		if (result.Equals (CoinResult.Shield)) {
			GUIController.instance.ShowEnemyGoesFirstRibbon ();
			opponentPlayer.stats.ShowGoFirstToken ();
		} else if(result.Equals (CoinResult.Sword)){
			GUIController.instance.ShowYouGoFirstRibbon ();
			localPlayer.stats.ShowGoFirstToken ();
		}

		ChangeGameToMixedCardSetupState ();

		SoundManager.instance.PlayYourTurn ();
	}

	private void ChangeGameToHeadsAndTailsState() {
		gameState = GameState.HeadsAndTailsState;

		StartCoroutine (ChangeGameToHeadsAndTailsStateRoutine());
	}

	private void ChangeGameToMixedCardSetupState() {
		gameState = GameState.MixedCardSetupState;

		StartCoroutine (ChangeGameToMixedCardSetupStateRoutine());
	}

	private void ChangeGameToGamePlayState() {
		gameState = GameState.GamePlayState;
	}

	public void EndPutToMixedCardPile() {
		ChangeGameToGamePlayState ();
		messageManager.ShowDrawACardMessage ();

		localPlayer.mixedCard.gameObject.SetActive (false);
		localPlayer.currentCard = null;

		GUIController.instance.ShowMixedCardButton ();
	}

	public void EndPutToSavedCardPile() {
		ChangeGameToGamePlayState ();

		localPlayer.savedCard.gameObject.SetActive (false);
		localPlayer.currentCard = null;

		GUIController.instance.ShowSavedCardButton ();

		DrawTopCardFromDeck ();
	}

	private void ResetPlayersPosition() {
		localPlayer.transform.position = GUIController.instance.GetDeckButtonTransformedPosition ();
		opponentPlayer.transform.position = GUIController.instance.GetOpponentShieldTransformPosition ();
	}

	IEnumerator ChangeGameToHeadsAndTailsStateRoutine() {
		//GUIController.instance.FadeInInteractionBlocker ();

		yield return new WaitForSeconds (1);

		if(coin != null) {
			coin.Enable ();
			coin.FadeIn ();

			while(!coin.FadedIn) { 
				yield return null;
			}

			yield return new WaitForSeconds (1);

			coin.FlipCoin ();
		}
	}

	IEnumerator ChangeGameToMixedCardSetupStateRoutine() {
		//GUIController.instance.FadeInInteractionBlocker ();

		yield return new WaitForSeconds (2);

		DrawMixedCardFromDeck ();
	}

	IEnumerator CallSpellCastingBySpellSelection(Card selectedCard, SpellSelection selection) {
		if(spellManager != null && selectedCard != null) {
			spellManager.CastSpell (selectedCard.element, selection, opponentPlayer, localPlayer);
		}

		yield return new WaitForSeconds (2);
	}
}