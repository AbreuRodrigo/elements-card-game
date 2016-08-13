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
	public SpellTypeEffectManager spellTypeEffectManager;
	public int currentTurn = 1;
	public bool invertGoesFirstOnTurnEnd;

	private bool endedTurn;
	private Card previousCardPlayed;

	private int dice1Result = 0;
	private int dice2Result = 0;
	private int dice3Result = 0;
	private int diceIndex = 0;

	public int Dice1Result {
		get { return dice1Result; }
	}
	public int Dice2Result {
		get { return dice2Result; }
	}
	public int Dice3Result {
		get { return dice3Result; }
	}

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

		localPlayer.opponent = opponentPlayer;
		opponentPlayer.opponent = localPlayer;
	}

	public void PressEndTurnButton() {
		SoundManager.instance.PlayClickSound ();

		StartCoroutine (EndTurnRoutine());
	}

	public void PressDeckButtonLogics() {
		if(IsGamePlayState() && localPlayer.currentCard == null && !localPlayer.skipThisTurn) {
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
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Circle);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressSquareActionButton() {
		SoundManager.instance.PlayClickSound ();

		if(localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Square);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressRhombusActionButton() {
		SoundManager.instance.PlayClickSound ();

		if(localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			GUIController.instance.HideCardActionButtons();
			localPlayer.currentCard.PutIntoGameAsLocalPlayer(SpellSelection.Rhombus);
			ShowBluePlayerArcaneCircle ();
		}
	}

	public void PressSaveCardActionButton() {
		SoundManager.instance.PlayClickSound ();

		if (localPlayer.currentCard != null && localPlayer.currentCard.IsCardStateInHand()) {
			GUIController.instance.HideCardActionButtons ();

			Card tempCard = localPlayer.currentCard;

			if(localPlayer.savedCard != null &&  localPlayer.savedCard.IsCardStateSaved()) {
				ChangeToChangingSavedCardState ();

				localPlayer.savedCard.gameObject.SetActive (true);
				localPlayer.savedCard.MoveMeToCenter ();
				localPlayer.savedCard.ChangeToInHandState ();
				localPlayer.currentCard = localPlayer.savedCard;

				GUIController.instance.HideSavedCardButton ();
			}

			localPlayer.savedCard = tempCard;
			localPlayer.savedCard.PutToSavedState ();
		}
	}

	public void PressPutToMixActionButton() {
		SoundManager.instance.PlayClickSound ();

		if (localPlayer.currentCard != null) {
			GUIController.instance.HideCardActionButtons ();

			if (localPlayer.currentCard.IsCardStateMixedSelection ()) {				
				localPlayer.currentCard.PutIntoWaitingToMixState ();
			}else if(localPlayer.currentCard.IsCardStateInHand()) {
				StartCoroutine (StartCardMixingProcess());
			}
		}
	}

	public void ShowBluePlayerArcaneCircle() {
		if (bluePlayerArcaneCircle != null) {
			bluePlayerArcaneCircle.ShowArcaneCircle ();
			ProcessSpellsInGame ();
		}
	}

	public void ShowRedPlayerArcaneCircle() {
		if (redPlayerArcaneCircle != null) {
			redPlayerArcaneCircle.ShowArcaneCircle ();
			PassTurnPhase ();
			ProcessSpellsInGame ();
		}		
	}

	public void HideBluePlayerArcaneCircle() {
		if (bluePlayerArcaneCircle != null) {
			bluePlayerArcaneCircle.HideArcaneCircle ();
		}
	}

	public void HideRedPlayerArcaneCircle() {
		if (redPlayerArcaneCircle != null) {
			redPlayerArcaneCircle.HideArcaneCircle ();
		}		
	}

	public void ViewMixedCard() {
		if(localPlayer != null && !IsMixedCardSetupState()) {
			GUIController.instance.HideMixedCardButton ();

			if(localPlayer.currentCard != null) {
				localPlayer.hiddenCard = localPlayer.currentCard;
				localPlayer.hiddenCard.gameObject.SetActive (false);
			}

			if(localPlayer.mixedCard != null) {
				localPlayer.currentCard = localPlayer.mixedCard;
				localPlayer.mixedCard = null;
				localPlayer.currentCard.gameObject.SetActive (true);
				localPlayer.currentCard.ChangeToMixedSelectionState ();
				localPlayer.currentCard.MoveMeToCenter ();
			}
		}
	}

	public void ViewSavedCard() {
	
	}

	//Notificated by coin when whenever it's flipped
	public void NotificationFromCoinFlip(CoinResult result) {
		if(IsHeadsAndTailsState()) {
			CoinNotificationForHeadsAndTailState (result);
		}
		if(IsGamePlayState()) {
			Debug.Log (result);
		}

		if(result.Equals(CoinResult.Sword)) {
			coin.FadeOutFromSword ();
		}else if(result.Equals(CoinResult.Shield)) {
			coin.FadeOutFromShield ();
		}
	}

	private void CoinNotificationForHeadsAndTailState(CoinResult result) {
		if(localPlayer != null && localPlayer.stats != null) {
			localPlayer.stats.ShowPlayerStats ();
		}

		if(opponentPlayer != null && opponentPlayer.stats != null) {
			opponentPlayer.stats.ShowPlayerStats ();
		}

		ChangeGameStateByFirstCoinFlippingResult (result);

		GUIController.instance.ShowCardBases ();
	}

	public void NotificationFromDiceRoll1(int diceFace) {
		dice1Result = diceFace;
	}

	public void NotificationFromDiceRoll2(int diceFace) {
		dice2Result = diceFace;
	}

	public void NotificationFromDiceRoll3(int diceFace) {
		dice3Result = diceFace;
	}

	public void ResetDice() {
		dice1Result = 0;
		dice2Result = 0;
		dice3Result = 0;
	}

	//PROCESS CARD SPELLS IN GAME
	private void ProcessSpellsInGame() {
		spellsInGame++;

		if(spellsInGame == 2) {
			PassTurnPhase ();

			if(turnManager.IsCombatPhase()) {
				if (opponentPlayer.goesFirst) {
					StartCoroutine (OpponentGoesFirst ());
				}
				if(localPlayer.goesFirst) {
					StartCoroutine (LocalPlayerGoesFirst ());
				}
			}

			spellsInGame = 0;
		}
	}

	public void PassTurnPhase() {
		if (turnManager != null) {
			turnManager.NextPhase ();
		}
	}

	public bool TakeAChanceUnder(int chance) {
		return Random.Range (0, 100) < chance;
	}

	public void Roll1Die() {
		dice1Result = diceManager.AdvancedDieResult ();
		diceManager.Roll1Die (dice1Result);
	}

	public void Roll2Dice() {
		dice1Result = diceManager.AdvancedDieResult ();
		dice2Result = diceManager.AdvancedDieResult ();

		diceManager.Roll2Dice (dice1Result, dice2Result);
	}

	public void Roll3Dice() {
		dice1Result = diceManager.AdvancedDieResult ();
		dice2Result = diceManager.AdvancedDieResult ();
		dice3Result = diceManager.AdvancedDieResult ();

		diceManager.Roll3Dice (dice1Result, dice2Result, dice3Result);
	}

	public void CastSpell(Card selectedCard, bool hideCards, Player target, Player source) {
		if(selectedCard != null && !source.skipThisTurn) {
			if(!selectedCard.element.Equals(CardElement.Wild)) {
				StartCoroutine (CallSpellCastingBySpellSelection (selectedCard, hideCards, target, source));
			}
		}
	}

	public void PutAiOpponentCardIntoGame() {
		if(IsHumanVSMachineGameMode()) {
			if (!opponentPlayer.skipThisTurn) {
				aiAgent.AIDrawTopCardFromDeck ();
			}
		}
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
		//DeckPatternManager.instance.BuildWaterDeck (localPlayer);
		DeckPatternManager.instance.BuildDeckInUse (localPlayer);
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
			//DeckPatternManager.instance.BuildWaterDeck (opponentPlayer);

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

	private bool IsChangingSavedCardState() {
		return gameState.Equals (GameState.ChangingSavedCardStated);
	}

	//State Transitions
	private void ChangeGameStateByFirstCoinFlippingResult(CoinResult result) {
		if (result.Equals (CoinResult.Shield)) {
			GUIController.instance.ShowEnemyGoesFirstRibbon ();
			opponentPlayer.stats.ShowGoFirstToken ();
			opponentPlayer.goesFirst = true;
			localPlayer.goesFirst = false;
		} else if(result.Equals (CoinResult.Sword)) {
			GUIController.instance.ShowYouGoFirstRibbon ();
			localPlayer.stats.ShowGoFirstToken ();
			localPlayer.goesFirst = true;
			opponentPlayer.goesFirst = false;
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

	private void ChangeToChangingSavedCardState() {
		gameState = GameState.ChangingSavedCardStated;
	}

	public void EndPutToMixedCardPile() {
		ChangeGameToGamePlayState ();
		messageManager.ShowDrawACardMessage ();

		if(localPlayer.mixedCard == null && localPlayer.currentCard != null) {
			localPlayer.mixedCard = localPlayer.currentCard;
		}

		localPlayer.mixedCard.gameObject.SetActive (false);
		localPlayer.currentCard = null;

		if(localPlayer.hiddenCard != null) {
			localPlayer.currentCard = localPlayer.hiddenCard;
			localPlayer.hiddenCard = null;
			localPlayer.currentCard.gameObject.SetActive (true);
			localPlayer.currentCard.Float ();
		}

		GUIController.instance.ShowMixedCardButton ();
	}

	public void EndPutToDiscardedCardPile() {
		localPlayer.attackProtection = null;
		opponentPlayer.attackProtection = null;

		GUIController.instance.ShowEndTurnButton ();
	}

	public void EndPutToSavedCardPile() {
		localPlayer.savedCard.gameObject.SetActive (false);

		if (!IsChangingSavedCardState ()) {
			localPlayer.currentCard = null;
			DrawTopCardFromDeck ();
		}

		ChangeGameToGamePlayState ();

		GUIController.instance.ShowSavedCardButton ();
	}

	private void ResetPlayersPosition() {
		localPlayer.transform.position = GUIController.instance.GetDeckButtonTransformedPosition ();
		opponentPlayer.transform.position = GUIController.instance.GetOpponentShieldTransformPosition ();
	}

	private void RevealOpponentFirst() {
		if (opponentPlayer.currentCard != null) {
			opponentPlayer.currentCard.EnemyReveal ();
		}
		redPlayerArcaneCircle.HideArcaneCircle ();
	}

	private void RevealLocalPlayerFirst() {
		if (localPlayer.currentCard != null) {
			localPlayer.currentCard.LocalPlayerReveal ();
		}
		bluePlayerArcaneCircle.HideArcaneCircle ();	
	}

	IEnumerator StartCardMixingProcess() {
		localPlayer.currentCard.DeselectCard ();

		localPlayer.currentCard.MoveFromInHandToRightAndStartMixing ();

		localPlayer.savedCard.gameObject.SetActive (true);

		GUIController.instance.HideSavedCardButton ();

		localPlayer.savedCard.MoveFromSavedToLeftAndStartMixing ();

		yield return new WaitForSeconds (1.5f);

		localPlayer.currentCard.MoveMeToCenter ();
		localPlayer.savedCard.MoveMeToCenter ();

		yield return new WaitForSeconds (1);

		ElementEffectManager.instance.InvokeMixedEffectByRequirements (localPlayer.currentCard.element, localPlayer.savedCard.element);

		localPlayer.currentCard.ChangeToDiscardedState ();
		localPlayer.currentCard.gameObject.SetActive (false);

		localPlayer.savedCard.ChangeToDiscardedState ();
		localPlayer.savedCard.gameObject.SetActive (false);

		localPlayer.mixedCard.gameObject.SetActive (true);
		localPlayer.mixedCard.FadeIn ();

		localPlayer.currentCard = localPlayer.mixedCard;
		localPlayer.currentCard.ChangeToInHandState ();

		localPlayer.savedCard = null;
		localPlayer.mixedCard = null;

		GUIController.instance.HideMixedCardButton ();
	}

	IEnumerator ChangeGameToHeadsAndTailsStateRoutine() {
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
		yield return new WaitForSeconds (2);
		DrawMixedCardFromDeck ();
	}

	IEnumerator CallSpellCastingBySpellSelection(Card selectedCard, bool hideCards, Player target, Player source) {
		if(spellManager != null && selectedCard != null) {
			SpellResponse response = spellManager.PreviewSpell (selectedCard.element, selectedCard.selectedSpell);
			bool mockedEffect = source.lastSpellCasted != null && response.mockEffect;

			if (mockedEffect) {//For spell copy
				SpellResponse mockedResponse = spellManager.PreviewSpell (
					source.lastSpellCasted.Element, source.lastSpellCasted.SelectedSpell
				);

				if (spellTypeEffectManager != null) {
					spellTypeEffectManager.ShowSpellType (mockedResponse.spellType, mockedResponse.spellName, source.currentCard.transform.position);
				}	
			} else {//Normal spell flow
				if (spellTypeEffectManager != null) {
					spellTypeEffectManager.ShowSpellType (response.spellType, response.spellName, source.currentCard.transform.position);
				}
			}

			ElementEffectManager.instance.ThrowElementEffect (
				mockedEffect ? source.lastSpellCasted.Element : selectedCard.element, 
				mockedEffect ? source.lastSpellCasted.SelectedSpell : selectedCard.selectedSpell,
				source, selectedCard.gameObject.transform.position, 
				GUIController.instance.RectToTransformedPosition(target.shieldRectTransform)
			);

			if (source.lastSpellCasted == null) {
				source.lastSpellCasted = new SpellHistoryData (selectedCard.element, selectedCard.selectedSpell);
			} else {
				source.lastSpellCasted.Reset (selectedCard.element, selectedCard.selectedSpell);
			}

			if (target.HasAttackProtection && !response.bypass) {
				if (response.spellType.Equals (target.protectionType)) {
					target.attackProtection.GetComponent<Collider> ().enabled = true;

					if (target.lastSpellCasted.Element.Equals (CardElement.Earth)) {
						source.Debuffs.AddKnockDown ();
					}
					if (target.lastSpellCasted.Element.Equals (CardElement.Ice)) {
						source.Debuffs.AddFreeze ();
					}
				} else {
					target.attackProtection.GetComponent<Collider> ().enabled = false;
					spellManager.CastSpell (selectedCard.element, selectedCard.selectedSpell, target, source);

					if(response.spellType.Equals(SpellType.Melee) && target.IsStatic()) {
						source.DecreaseHP (2);
					}
				}
			} else {
				if(mockedEffect) {
					spellManager.CastSpell (source.lastSpellCasted.Element, source.lastSpellCasted.SelectedSpell, target, source);
				}else {
					spellManager.CastSpell (selectedCard.element, selectedCard.selectedSpell, target, source);
				}
				if(response.spellType.Equals(SpellType.Melee) && target.IsStatic()) {
					source.DecreaseHP (2);
				}
			}
		}

		yield return new WaitForSeconds (1f);

		if(!hideCards) {
			yield return null;	
		}else {
			yield return new WaitForSeconds (1f);

			if (localPlayer.currentCard != null) {
				localPlayer.currentCard.PutIntoDiscardedState ();
			}
			if (opponentPlayer.currentCard != null) {
				opponentPlayer.currentCard.ScaleOut ();
			}
		}
	}

	IEnumerator OpponentGoesFirst() {
		yield return new WaitForSeconds (2);

		if (!opponentPlayer.skipThisTurn) {
			RevealOpponentFirst ();
			yield return new WaitForSeconds (1);
			CastSpell (opponentPlayer.currentCard, localPlayer.skipThisTurn, localPlayer, opponentPlayer);
		}

		yield return new WaitForSeconds (0.5f);

		if(!localPlayer.skipThisTurn) {
			RevealLocalPlayerFirst ();
			yield return new WaitForSeconds (1);
			CastSpell(localPlayer.currentCard, true, opponentPlayer, localPlayer);
		}

		PassTurnPhase ();

		yield return new WaitForSeconds (1);
	}

	IEnumerator LocalPlayerGoesFirst() {
		yield return new WaitForSeconds (2);

		if (!localPlayer.skipThisTurn) {
			RevealLocalPlayerFirst ();
			yield return new WaitForSeconds (1);
			CastSpell (localPlayer.currentCard, opponentPlayer.skipThisTurn, opponentPlayer, localPlayer);
		}

		yield return new WaitForSeconds (0.5f);

		if (!opponentPlayer.skipThisTurn) {
			RevealOpponentFirst ();
			yield return new WaitForSeconds (1);
			CastSpell (opponentPlayer.currentCard, true, localPlayer, opponentPlayer);
		}

		PassTurnPhase ();

		yield return new WaitForSeconds (1);
	}

	IEnumerator EndTurnRoutine() {
		if (!endedTurn) {
			endedTurn = true;
			yield return new WaitForSeconds (0.5f);

			GUIController.instance.HideEndTurnButton ();

			SoundManager.instance.PlayYourTurn ();

			PassTurnPhase ();
			endedTurn = false;

			diceManager.ResetDice ();

			previousCardPlayed = null;

			yield return new WaitForSeconds (0.5f);

			GUIController.instance.HideInteractionBlocker ();

			localPlayer.currentCard = null;
			opponentPlayer.currentCard = null;

			if(!localPlayer.skipNextTurn && localPlayer.skipThisTurn) {
				localPlayer.skipThisTurn = false;
			}
			if(!opponentPlayer.skipNextTurn && opponentPlayer.skipThisTurn) {
				opponentPlayer.skipThisTurn = false;
			}

			currentTurn++;

			localPlayer.EndPlayerTurn ();
			opponentPlayer.EndPlayerTurn ();

			if(invertGoesFirstOnTurnEnd) {
				localPlayer.goesFirst = !localPlayer.goesFirst;
				opponentPlayer.goesFirst = !opponentPlayer.goesFirst;
				invertGoesFirstOnTurnEnd = false;
			}

			if(!localPlayer.skipNextTurn && !opponentPlayer.skipNextTurn) {
				DrawTopCardFromDeck ();
			} else {
				if (localPlayer.skipNextTurn && !opponentPlayer.skipNextTurn) {
					StartCoroutine(ApplyLogicsForLocalPlayerSkipCurrentTurn ());
				} else if (opponentPlayer.skipNextTurn && !localPlayer.skipNextTurn) {
					StartCoroutine(ApplyLogicsForOpponentPlayerSkipCurrentTurn ());
				} else { //Otherwise, follows the normal flow
					SetUpPlayerForSkippingNextTurn(localPlayer);
					SetUpPlayerForSkippingNextTurn(opponentPlayer);

					currentTurn++;

					DrawTopCardFromDeck ();
				}
			}
		}
	}

	IEnumerator ApplyLogicsForLocalPlayerSkipCurrentTurn() {
		SetUpPlayerForSkippingNextTurn (localPlayer);

		GUIController.instance.ShowInteractionBlockerHalfFaded ();

		ProcessSpellsInGame ();

		PutAiOpponentCardIntoGame ();

		yield return new WaitForSeconds (6.5f);

		opponentPlayer.currentCard.ScaleOut ();

		yield return new WaitForSeconds (1f);

		GUIController.instance.ShowEndTurnButton ();
	}

	IEnumerator ApplyLogicsForOpponentPlayerSkipCurrentTurn() {
		SetUpPlayerForSkippingNextTurn (opponentPlayer);

		DrawTopCardFromDeck ();

		PassTurnPhase ();

		ProcessSpellsInGame ();

		while(spellsInGame <= 2) {
			yield return null;
		}

		yield return new WaitForSeconds (5);

		localPlayer.currentCard.PutIntoDiscardedState ();

		yield return new WaitForSeconds (1);

		GUIController.instance.ShowEndTurnButton ();
	}

	private void SetUpPlayerForSkippingNextTurn(Player player) {
		if (player.skipNextTurn) {
			player.skipThisTurn = true;
			player.skipNextTurn = false;
			if (player.IsKnockedDown ()) {
				player.Debuffs.RemoveKnockDown ();
			}
		}
	}

	private void ClearPlayerTurnSkipping(Player player) {
		player.skipThisTurn = false;
		player.skipNextTurn = false;
		if (player.IsKnockedDown ()) {
			player.Debuffs.RemoveKnockDown ();
		}
	}
}