using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class GamePlayController : MonoBehaviour {
	public static GamePlayController instance;

	[Header("Configurations")]
	public GameMode gameMode;
	public GameState gameState;
	public bool useDefaultDeck = true;
	public bool hasSetupMixedCards;
	public bool flowIsOnHold;
	public int spellsInGame;

	[Header("Players")]
	public Player localPlayer;
	public Player opponentPlayer;
	private AIAgent aiAgent;
	private Player peekedTarget;

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
    public SpellFailedMsg spellFailedMsg;
	public NoSavedCardPlaceHolder noSavedCardPlaceHolder;
	public int currentTurn = 1;
	public bool invertGoesFirstOnTurnEnd;

	private InterstitialAd interstitial;

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

		RequestInterstitial ();
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

		interstitial.Show ();
	}

	public void PressEndTurnButton() {
		SoundManager.instance.PlayClickSound ();

		StartCoroutine (EndTurnRoutine());
	}

	public void PressEndPeekButton() {
		SoundManager.instance.PlayClickSound ();

		EndPeekingMode ();
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

	public void NotificationFromPlayerDestroyed(Player player) {
		if (player.localPlayer) {//Local player lost the game
			GUIController.instance.ShowGameOverRibbon();
		} else {
			GUIController.instance.ShowVictoryRibbon();
		}

		localPlayer.HideCurrentCard ();
		opponentPlayer.HideCurrentCard ();

		ChangeToGameOverState ();

		if (interstitial != null && interstitial.IsLoaded()) {
			interstitial.Show();
		}

		StartCoroutine (WaitEndGoBackToMenu ());
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

			ValidateTurnPriority ();

			if(turnManager.IsCombatPhase()) {
				if (opponentPlayer.goesFirst) {
					StartCoroutine (OpponentGoesFirst ());
				}else if(localPlayer.goesFirst) {
					StartCoroutine (LocalPlayerGoesFirst ());
				}
			}

			spellsInGame = 0;
		}
	}

	private void ValidateTurnPriority() {
		SpellResponse opponentResponse = null;
		SpellResponse localPlayerResponse = null;

		if (opponentPlayer != null && opponentPlayer.currentCard != null) {
			opponentResponse = spellManager.PreviewSpell (opponentPlayer.currentCard);
		}

		if(localPlayer != null && localPlayer.currentCard != null) {
			localPlayerResponse = spellManager.PreviewSpell (localPlayer.currentCard);
		}

		if (!invertGoesFirstOnTurnEnd) {//NEW
			if (localPlayer.goesFirst) {
				if (opponentResponse != null && opponentResponse.alwaysGoesFirst) {
					invertGoesFirstOnTurnEnd = true;
					localPlayer.goesFirst = false;
					opponentPlayer.goesFirst = true;
				}
			} else {
				if (localPlayerResponse != null && localPlayerResponse.alwaysGoesFirst) {
					invertGoesFirstOnTurnEnd = true;
					localPlayer.goesFirst = true;
					opponentPlayer.goesFirst = false;
				}
			}
		}
	}

	public void PassTurnPhase() {
		if (turnManager != null && !IsGameOverState()) {
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

	public void StartPeekingMode(Player target) {
		peekedTarget = target;

		if (IsGamePlayState () && peekedTarget != null) {
			List<Card> peeked = peekedTarget.Deck.PeekNextNCards (2);

			bool flip = !peekedTarget.localPlayer;

			localPlayer.HideCurrentCard ();
			HideBluePlayerArcaneCircle ();

			opponentPlayer.HideCurrentCard ();
			HideRedPlayerArcaneCircle ();

			if (peekedTarget.HasSavedCard) {
				GenericPeekCard (peekedTarget, peekedTarget.savedCard, false);
				peekedTarget.savedCard.MoveMeToPeekPositionOne (flip);

				if (peekedTarget.localPlayer) {
					GUIController.instance.HideSavedCardButton ();
				}
			} else if(noSavedCardPlaceHolder != null) {
				noSavedCardPlaceHolder.Show ();
			}

			if(peeked != null && peeked.Count > 0) {
				if(peeked[0] != null) {
					GenericPeekCard (peekedTarget, peeked [0], true);
					peeked [0].MoveMeToPeekPositionTwo (flip);
				}
				if(peeked[1] != null) {
					GenericPeekCard (peekedTarget, peeked [1], true);
					peeked [1].MoveMeToPeekPositionThree (flip);
				}
			}

			if (!peekedTarget.localPlayer) {
				GUIController.instance.ShowPeekingModeButton ();
			} else {
				StartCoroutine (WaitToEndOpponentPeeking ());
			}
		}
	}

	public void EndPeekingMode() {
		if(IsGamePlayState () && peekedTarget != null) {
			List<Card> peeked = peekedTarget.Deck.PeekNextNCards (2);

			bool flip = !peekedTarget.localPlayer;

			if (peekedTarget.HasSavedCard) {
				peekedTarget.savedCard.RewindToPositionBeforePeek (flip, 0);
				peekedTarget.savedCard.ChangeToSavedState ();
			} else if(noSavedCardPlaceHolder != null) {
				noSavedCardPlaceHolder.Hide ();
			}

			if(peeked != null && peeked.Count > 0) {
				if(peeked[0] != null) {
					peeked [0].ChangeToOnDeckState ();
					peeked [0].RewindToPositionBeforePeek (flip, 0.5f);
				}
				if(peeked[1] != null) {
					peeked [1].ChangeToOnDeckState ();
					peeked [1].RewindToPositionBeforePeek (flip, 1.0f);
				}
			}

			if (!peekedTarget.localPlayer) {
				GUIController.instance.HidePeekingModeButton ();
			} else {
				GUIController.instance.DeckButtonTurnOn ();

				if(peekedTarget.HasSavedCard) {
					GUIController.instance.ShowSavedCardButton ();
				}
			}

			StartCoroutine (FinishPeekingMode ());
		}
	}

	private void RequestInterstitial() {
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1033173712";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	private void GenericPeekCard(Player target, Card card, bool validateHasNextCard) {
		card.gameObject.SetActive (true);
		card.ChangeToPeekedState ();

		if (validateHasNextCard && !target.Deck.HasNext () && target.localPlayer) {
			GUIController.instance.DeckButtonTurnOff ();
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

	private void ChangeToGameOverState() {
		gameState = GameState.GameOverState;
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

	private void RevealOpponent() {
		if (opponentPlayer.currentCard != null && !IsGameOverState()) {
			opponentPlayer.currentCard.EnemyReveal ();
		}
		redPlayerArcaneCircle.HideArcaneCircle ();
	}

	private void RevealLocalPlayer() {
		if (localPlayer.currentCard != null && !IsGameOverState()) {
			localPlayer.currentCard.LocalPlayerReveal ();
		}
		bluePlayerArcaneCircle.HideArcaneCircle ();	
	}

	IEnumerator WaitEndGoBackToMenu() {
		yield return new WaitForSeconds (4);

		GUIMenuController.instance.FadeScreenOut ();

		yield return new WaitForSeconds (3);

		SoundManager.instance.ChangeToBattleMusic ();

		yield return new WaitForSeconds (2);

		SceneManager.LoadScene ("Menu");
	}

	IEnumerator WaitToEndOpponentPeeking() {
		int t = Random.Range (2, 4);

		yield return new WaitForSeconds (t);

		EndPeekingMode ();
	}

	IEnumerator FinishPeekingMode() {
		yield return new WaitForSeconds (2);

		localPlayer.ShowCurrentCard ();
		opponentPlayer.ShowCurrentCard ();

		if(peekedTarget.localPlayer) {
			GUIController.instance.DeckButtonTurnOn ();
			peekedTarget = null;
		}

		flowIsOnHold = false;
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
		if (!IsGameOverState ()) {
			if (spellManager != null && selectedCard != null) {
				SpellResponse response = spellManager.PreviewSpell (selectedCard.element, selectedCard.selectedSpell);
				bool mockedEffect = target.lastSpellCasted != null && response.mockEffect;

				bool failed = false;

				if("Scorch".Equals(response.spellName) && selectedCard.element.Equals(CardElement.Fire)) {
					failed = !TakeAChanceUnder (50);
				}
				if("Copy".Equals(response.spellName) && selectedCard.element.Equals(CardElement.Dark) &&
					target.lastSpellCasted == null) {
					failed = true;
				}

				if (source.lastSpellCasted == null) {
					source.lastSpellCasted = new SpellHistoryData (selectedCard.element, selectedCard.selectedSpell, response.spellType);
				} else {
					source.lastSpellCasted.Reset (selectedCard.element, selectedCard.selectedSpell, response.spellType);
				}

				if ("Copy".Equals (response.spellName) || source.currentCard.type.Equals(CardType.Mixed)) {
					source.lastSpellCasted = null;
				}

				if (!flowIsOnHold && spellsInGame < 2) {
					flowIsOnHold = response.flowIsOnHold;
				}

				int chance = Random.Range (0, 100);

				if ((source.Debuffs.IsBlind && (response.IsMeleeSpell || response.IsSpecialSpell) && chance <= 50) || failed) {
					spellFailedMsg.transform.position = source.currentCard.transform.position;
					spellFailedMsg.Show ();
					flowIsOnHold = false;
					source.lastSpellCasted = null;
				} else {
					if (mockedEffect) {//For spell copy
						SpellResponse mockedResponse = 
							spellManager.PreviewSpell (target.lastSpellCasted.Element, target.lastSpellCasted.SelectedSpell);

						flowIsOnHold = mockedResponse.flowIsOnHold;

						if (spellTypeEffectManager != null) {
							spellTypeEffectManager.ShowSpellType (mockedResponse.spellType, mockedResponse.spellName, source.currentCard.transform.position);
						}
					} else {//Normal spell flow
						if (spellTypeEffectManager != null) {
							spellTypeEffectManager.ShowSpellType (response.spellType, response.spellName, source.currentCard.transform.position);
						}
					}

					CardElement element;
					SpellSelection selectedSpell;

					if(mockedEffect && target.lastSpellCasted != null) {
						element = target.lastSpellCasted.Element;
						selectedSpell = target.lastSpellCasted.SelectedSpell;
					}else {
						element = selectedCard.element;
						selectedSpell = selectedCard.selectedSpell;
					}

					ElementEffectManager.instance.ThrowElementEffect (element, selectedSpell, source, 
						selectedCard.gameObject.transform.position, 
						GUIController.instance.RectToTransformedPosition (target.shieldRectTransform)
					);

					if (target.HasAttackProtection && !response.bypass) {
						if (response.spellType.Equals (target.protectionType)) {
							target.attackProtection.GetComponent<Collider> ().enabled = true;

							if (target.lastSpellCasted.Element.Equals (CardElement.Earth)) {
								source.Debuffs.AddKnockDown ();
							}
							if (target.lastSpellCasted.Element.Equals (CardElement.Ice)) {
								source.Debuffs.AddFreeze ();
								flowIsOnHold = false;
							}
						} else {
							target.attackProtection.GetComponent<Collider> ().enabled = false;
							spellManager.CastSpell (selectedCard.element, selectedCard.selectedSpell, target, source);
						}
					} else {
						if (mockedEffect) {
							spellManager.CastSpell (target.lastSpellCasted.Element, target.lastSpellCasted.SelectedSpell, target, source);
						} else {
							spellManager.CastSpell (selectedCard.element, selectedCard.selectedSpell, target, source);
						}
					}

					if(source.lastSpellCasted != null && 
						source.lastSpellCasted.SpellType.Equals(SpellType.Melee) && target.IsStatic() && target.lastDamageReceived > 0) {
						source.DecreaseHP (2);
					}
				}
			}

			float mod = 1;

			if (flowIsOnHold) {
				mod = 0.5f;

				while (flowIsOnHold) {
					yield return null;
				}
			}

			yield return new WaitForSeconds (mod);

			if (hideCards) {
				yield return new WaitForSeconds (mod);

				if (localPlayer.currentCard != null) {
					localPlayer.currentCard.PutIntoDiscardedState ();
				}
				if (opponentPlayer.currentCard != null) {
					opponentPlayer.currentCard.ScaleOut ();
				}
			}
		} else {
			yield return null;
		}
	}

	IEnumerator OpponentGoesFirst() {
		yield return new WaitForSeconds (2);

		if (!opponentPlayer.skipThisTurn) {
			RevealOpponent ();
			yield return new WaitForSeconds (1);
			CastSpell (opponentPlayer.currentCard, localPlayer.skipThisTurn, localPlayer, opponentPlayer);
		}

		yield return new WaitForSeconds (0.5f);

		while (flowIsOnHold) {
			yield return null;
		}

		if (!IsGameOverState ()) {
			if (!localPlayer.skipThisTurn) {
				RevealLocalPlayer ();
				yield return new WaitForSeconds (1);
				CastSpell (localPlayer.currentCard, true, opponentPlayer, localPlayer);
			}

			PassTurnPhase ();
		}

		yield return new WaitForSeconds (1);
	}

	IEnumerator LocalPlayerGoesFirst() {
		yield return new WaitForSeconds (2);

        if (!localPlayer.skipThisTurn) {
			RevealLocalPlayer ();
			yield return new WaitForSeconds (1);
			CastSpell (localPlayer.currentCard, opponentPlayer.skipThisTurn, opponentPlayer, localPlayer);
		}

		yield return new WaitForSeconds (0.5f);

		while (flowIsOnHold) {
			yield return null;
		}

		if (!IsGameOverState ()) {
			if (!opponentPlayer.skipThisTurn) {
				RevealOpponent ();
				yield return new WaitForSeconds (1);
				CastSpell (opponentPlayer.currentCard, true, localPlayer, opponentPlayer);
			}

			PassTurnPhase ();
		}

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
			flowIsOnHold = false;

			diceManager.ResetDice ();

			previousCardPlayed = null;

			localPlayer.lastDamageReceived = 0;
			opponentPlayer.lastDamageReceived = 0;

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