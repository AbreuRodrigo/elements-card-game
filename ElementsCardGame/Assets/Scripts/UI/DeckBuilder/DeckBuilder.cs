using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeckBuilder : MonoBehaviour {
	private const int DECK_SIZE_LIMIT = 32;
	private const float PAGE_DISTANCE = 630;

	public const string DECK2_FILE_NAME = "dk2";
	public const string DECK3_FILE_NAME = "dk3";

	[System.Serializable]
	public class SpriteByElement {
		public Sprite sprite;
		public CardElement element;
	}

	public static DeckBuilder Instance;

	public RectTransform decksViewport;
	public RectTransform pagesViewport;
	public Button arrowRight;
	public Button arrowLeft;

	public DraggingCard draggingCard;

	public DeckStructure[] decks = new DeckStructure[3];

	public RectTransform[] pages = new RectTransform[2];

	public CardMarkerView[] cardMarkers = new CardMarkerView[12];

	public RectTransform messageContainer;

	public ParticleSystem[] cardParticles = new ParticleSystem[2];
	private int cardParticlesIndex = 0;

	public SpriteByElement[] spritesByElement = new SpriteByElement[14];
	private Dictionary<CardElement, int> spritesByElementIndex = new Dictionary<CardElement, int> (14);

	public DeckChangeButton[] deckChangeButtons = new DeckChangeButton[3];

	private DeckData deck1;
	private DeckData deck2;
	private DeckData deck3;

	private int currentPage = 1;
	private float[] distances = new float[3];
	public int currentDeckIndex = -1;

	public Image interactionBlocker;

	private Dictionary<string, CardElement> cardElementByTextName;

	public CardView[] cardViews;
	private Dictionary<CardElement, int> cardViewIndexByElement;
	private Dictionary<CardElement, int> cardLimitPerDeck;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}

		cardElementByTextName = new Dictionary<string, CardElement>(14) {
			{"Blood", CardElement.Blood},
			{"Fire", CardElement.Fire},
			{"Dark", CardElement.Dark},
			{"Earth", CardElement.Earth},
			{"Water", CardElement.Water},
			{"Ice", CardElement.Ice},
			{"Nature", CardElement.Nature},
			{"Shadow", CardElement.Shadow},
			{"Light", CardElement.Light},
			{"Lightning", CardElement.Lightning},
			{"Magma", CardElement.Magma},
			{"Chaos", CardElement.Chaos},
			{"Zombie", CardElement.Zombie},
			{"Wild", CardElement.Wild}
		};

		cardViewIndexByElement = new Dictionary<CardElement, int> (14) {
			{CardElement.Blood, 0},
			{CardElement.Fire, 1},
			{CardElement.Dark, 2},
			{CardElement.Earth, 3},
			{CardElement.Ice, 4},
			{CardElement.Light, 5},
			{CardElement.Lightning, 6},
			{CardElement.Nature, 7},
			{CardElement.Shadow, 8},
			{CardElement.Water, 9},
			{CardElement.Magma, 10},
			{CardElement.Chaos, 11},
			{CardElement.Zombie, 12},
			{CardElement.Wild, 13}
		};

		cardLimitPerDeck = new Dictionary<CardElement, int> (14) {
			{CardElement.Blood, 5},
			{CardElement.Fire, 5},
			{CardElement.Dark, 3},
			{CardElement.Earth, 3},
			{CardElement.Ice, 3},
			{CardElement.Light, 3},
			{CardElement.Lightning, 5},
			{CardElement.Nature, 5},
			{CardElement.Shadow, 5},
			{CardElement.Water, 5},
			{CardElement.Magma, 1},
			{CardElement.Chaos, 1},
			{CardElement.Zombie, 1},
			{CardElement.Wild, 1}
		};
	}

	void Start() {
		ValidateDeckDatas ();
		StartCoroutine (WaitXSecondsAndStart(1.5f));
	}

	public void MoveUIToSpecificDeckStructureByIndex(int index) {
		currentDeckIndex = index;

		if(decksViewport != null) {
			StartCoroutine(SlideDeckStructure(index));
		}
	}

	public void PassCardsPageLeft() {
		currentPage++;

		SoundManager.instance.PlayClickSound ();

		if(currentPage == pages.Length) {
			StartCoroutine(PassCardsPage(-PAGE_DISTANCE));

			arrowRight.gameObject.SetActive (false);
			arrowLeft.gameObject.SetActive (true);
		}
	}

	public void StartDraggingCard(Sprite sprite, CardElement element, CardType type) {
		if(currentDeckIndex <= 0 || decks[currentDeckIndex].amountOfCards == DECK_SIZE_LIMIT) {
			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			p.z = 0;
			messageContainer.transform.position = p;

			return;
		}

		draggingCard.ResetSprite (sprite, element, type);
		draggingCard.CancelDragging ();
		draggingCard.Enable ();
	}

	public void CancelDraggingCard() {
		draggingCard.CancelDragging ();
	}

	public void PassCardsPageRight() {
		currentPage--;

		SoundManager.instance.PlayClickSound ();

		if(currentPage == 1) {
			StartCoroutine(PassCardsPage(PAGE_DISTANCE));

			arrowLeft.gameObject.SetActive (false);
			arrowRight.gameObject.SetActive (true);
		}
	}

	public void DisableArrowByCurrentPage() {
		if(currentPage == 1) {
			arrowRight.gameObject.SetActive (false);
		}
		if(currentPage == 2) { 
			arrowLeft.gameObject.SetActive (false);
		}
	}

	public void EnableArrowByCurrentPage () {
		if(currentPage == 1) {
			arrowRight.gameObject.SetActive (true);
		}
		if(currentPage == 2) { 
			arrowLeft.gameObject.SetActive (true);
		}
	}

	public void EmitCardParticles() {
		if(cardParticlesIndex < cardParticles.Length) {
			cardParticles [cardParticlesIndex].transform.position = draggingCard.transform.position;
			cardParticles [cardParticlesIndex].Play ();

			cardParticlesIndex++;

			if(cardParticlesIndex >= cardParticles.Length) {
				cardParticlesIndex = 0;
			}
		}
	}

	public void AddDraggingCardToCurrentDeck() {
		if(draggingCard != null) {
			GetDeckDataByIndex(currentDeckIndex).AddCard (new CardData(draggingCard.Element, 1, draggingCard.Type));

			decks [currentDeckIndex].AddCard (draggingCard.Element, draggingCard.Type);

			PulseCardMarkerOnAddingNewCard(currentDeckIndex);

			HandleCardViewVisibility (draggingCard.Element);
		}
	}

	public void RemoveCardMarkFromDeck(string element, int cardMarkIndex) {
		DeckStructure deckStructure = decks [currentDeckIndex];
		DeckData deckData = GetDeckDataByIndex (currentDeckIndex);

		deckStructure.hasMixed = deckData.mixedCardIndex > -1;
		deckStructure.hasWild = deckData.wildCardIndex > -1;

		if(deckStructure.amountOfCards > 0) {
			deckData.RemoveCard (cardMarkIndex);
			deckStructure.RemoveCard (cardElementByTextName [element]);
			ResetCardMarksPerSelectedDeckAfterRemoving (currentDeckIndex);
		}

		HandleCardViewVisibility (cardElementByTextName [element]);
	}

	public DeckData GetDefaultDeck() {
		if(deck1 == null) {
			InitializeDefaultDeck ();
		}

		return deck1;
	}
		
	private void HandleCardViewVisibility(CardElement element) {
		if(currentDeckIndex < 0) {
			currentDeckIndex = 0;
		}

		DeckData deck = GetDeckDataByIndex (currentDeckIndex);

		if(deck.CountElements(element) == cardLimitPerDeck[element]) {
			HideCardView (element);
		}
		if(deck.CountElements(element) < cardLimitPerDeck[element]) {
			ShowCardView (element);
		}
	}

	private void CleanupCardViewStates() {
		foreach (CardElement element in System.Enum.GetValues(typeof(CardElement))) {
			ShowCardView (element);
		}
	}

	private bool TestElementIsWild(CardElement element) {
		return CardElement.Wild.Equals (element);
	}

	private bool TestElementIsMixed(CardElement element) {
		return CardElement.Magma.Equals (element) || CardElement.Chaos.Equals (element) || CardElement.Zombie.Equals (element);
	}

	private void ShowCardView(CardElement element) {
		cardViews[cardViewIndexByElement [element]].gameObject.SetActive (true);
	}

	private void HideCardView(CardElement element) {
		cardViews [cardViewIndexByElement [element]].gameObject.SetActive (false);
	}

	private void SetupUIDistances() {
		for(int i = 0; i < decks.Length; i++) {
			distances [i] = decks [i].GetComponent<RectTransform> ().anchoredPosition.x * -1;
		}
	}

	private void ShowAllCardMarksPerSelectedDeck(int index) {
		StartCoroutine (ShowAllCardMarksPerSelectedDeckRoutine (index));
	}

	private void ResetCardMarksPerSelectedDeckAfterRemoving(int index) {
		StartCoroutine (ResetCardMarksPerSelectedDeckAfterRemovingRoutine (index));
	}

	private void PulseCardMarkerOnAddingNewCard(int index) {
		StartCoroutine (PulseCardMarkerOnAddingNewCardRoutine (index));
	}

	private void InitializeDefaultDeck() { 
		deck1 = new DeckData ("Default Deck");
		deck1.AddCard(new CardData(CardElement.Blood, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Fire, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Water, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Nature, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Earth, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Ice, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Shadow, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Light, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Lightning, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Dark, 3, CardType.Element));
		deck1.AddCard(new CardData(CardElement.Magma, 1, CardType.Mixed));
		deck1.AddCard(new CardData(CardElement.Wild, 1, CardType.Wild));
	}

	private void ValidateDeckDatas() {
		InitializeDefaultDeck ();//Just to be clear that it's setting up the deck1Structure

		deck2 = new DeckData ("Deck 2");
		deck3 = new DeckData ("Deck 3");

		if(PersistenceManager.Instance.FileExists(DECK2_FILE_NAME)) {
			deck2 = (DeckData) PersistenceManager.Instance.LoadData(DECK2_FILE_NAME);
		}
		if(PersistenceManager.Instance.FileExists(DECK3_FILE_NAME)) {
			deck3 = (DeckData) PersistenceManager.Instance.LoadData(DECK3_FILE_NAME);
		}

		decks [0].InitializeDeckStructure (deck1);
		decks [1].InitializeDeckStructure (deck2);
		decks [2].InitializeDeckStructure (deck3);
	}

	public DeckData GetDeckDataByIndex(int index) {
		if(index == 0) {
			return deck1;
		}else if(index == 1) {
			return deck2;
		}else if(index == 2) {
			return deck3;
		}

		return null;
	}

	private void AssembleCardMarkersByDeck(DeckData deck) {
		if (deck != null) {
			CardMarkerView marker;

			for (int i = 0; i < deck.Cards.Count; i++) {
				marker = cardMarkers [i];
				marker.orb.sprite = spritesByElement [spritesByElementIndex [deck.Cards [i].cardElement]].sprite;
				marker.cardName.text = deck.Cards [i].cardElement.ToString ();
				marker.amount.text = "" + deck.Cards [i].amount;
			}

			foreach(CardElement element in System.Enum.GetValues(typeof(CardElement))) {
				ShowCardView(element);
			}

			foreach(CardData cd in deck.Cards) {
				HandleCardViewVisibility (cd.cardElement);
			}
		}
	}

	IEnumerator ShowAllCardMarksPerSelectedDeckRoutine(int index) {
		if(decks != null) {
			DeckStructure deckStructure = decks [index];

			DeckData deck = GetDeckDataByIndex (index);

			AssembleCardMarkersByDeck (deck);

			for (int i = 0; i < deckStructure.cardMarkerSlots.Length; i++) {
				cardMarkers [i].gameObject.transform.position = deckStructure.cardMarkerSlots [i].position;

				if (i < deck.Cards.Count) {
					SoundManager.instance.PlayCardMark ();
					cardMarkers [i].ShowUp ();
				} else {
					cardMarkers [i].Idle ();
				}

				yield return new WaitForSeconds (0.05f);
			}
		}
	}

	IEnumerator ResetCardMarksPerSelectedDeckAfterRemovingRoutine(int index) {
		DeckStructure deckStructure = decks [index];

		DeckData deck = GetDeckDataByIndex (index);

		AssembleCardMarkersByDeck (deck);

		for (int i = 0; i < deckStructure.cardMarkerSlots.Length; i++) {
			cardMarkers [i].gameObject.transform.position = deckStructure.cardMarkerSlots [i].position;

			if(i < deck.Cards.Count) {
				cardMarkers [i].Pulse ();
			}else {
				cardMarkers [i].Idle ();
			}

			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator PulseCardMarkerOnAddingNewCardRoutine(int index) {
		if(decks [index].amountOfCards > 0) {
			DeckData deck = GetDeckDataByIndex (index);

			AssembleCardMarkersByDeck (deck);

			int i = deck.changedElementIndex;

			cardMarkers [i].gameObject.transform.position = decks [index].cardMarkerSlots [i].position;

			if (i < deck.Cards.Count) {
				cardMarkers [i].ShowUp ();
				SoundManager.instance.PlayCardMark ();
			}

			yield return new WaitForSeconds (0.01f);
		}
	}

	IEnumerator SlideDeckStructure(int index) {
		float timer = 0;
		float factor = 0.5f;
		float x = distances [index];
			
		Vector2 newPos = new Vector2 (x, decksViewport.anchoredPosition.y);

		SoundManager.instance.PlayCardMoveSound ();

		if (index > 0) {
			interactionBlocker.enabled = false;
		} else {
			interactionBlocker.enabled = true;
		}

		foreach(CardMarkerView cm in cardMarkers) {
			cm.Idle ();
		}

		while(timer < factor) {
			decksViewport.anchoredPosition = Vector2.Lerp (decksViewport.anchoredPosition, newPos, Time.deltaTime * 7);

			timer += Time.deltaTime;

			yield return new WaitForSeconds(0.01f);
		}

		decksViewport.anchoredPosition = newPos;

		ShowAllCardMarksPerSelectedDeck (index);
	}

	IEnumerator PassCardsPage(float displacement) {
		float timer = 0;
		float factor = 1;

		Vector2 newPos = new Vector3 (displacement + pagesViewport.anchoredPosition.x, pagesViewport.anchoredPosition.y);

		SoundManager.instance.PlayCardMoveSound ();

		while(timer < factor) {
			pagesViewport.anchoredPosition = Vector2.Lerp (pagesViewport.anchoredPosition, newPos, Time.deltaTime * 7);

			timer += Time.deltaTime;

			yield return new WaitForSeconds (0.01f);
		}

		pagesViewport.anchoredPosition = newPos;
	}

	IEnumerator WaitXSecondsAndStart(float time) {
		SetupUIDistances ();

		for(int i = 0; i < spritesByElement.Length; i++) {
			spritesByElementIndex [spritesByElement [i].element] = i;
		}

		int selectedDeck = PersistenceManager.Instance.GetPlayerDeckInUse ();

		if(selectedDeck > 0) {
			deckChangeButtons [selectedDeck].RunButtonChange ();
		}

		yield return new WaitForSeconds (time);

		ShowAllCardMarksPerSelectedDeck (selectedDeck);
	}
}