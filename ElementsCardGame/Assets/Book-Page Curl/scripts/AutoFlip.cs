using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour {
    public FlipMode mode;
    public float pageFlipTime;
    public float timeBetweenPages;
    public float delayBeforeStarting;
    public bool autoStartFlip;
	public Book controledBook;
    public int animationFramesCount;
    bool isFlipping;

	private Multiplier multiplier;

	class Multiplier {
		public float frameTime;
		public float xc;
		public float xl;
		public float h;
		public float dx;

		public Multiplier(AutoFlip autoFlip) {
			this.frameTime = autoFlip.pageFlipTime / autoFlip.animationFramesCount;
			this.xc = (autoFlip.controledBook.EndBottomRight.x + autoFlip.controledBook.EndBottomLeft.x) * 0.5f;
			this.xl = ((autoFlip.controledBook.EndBottomRight.x - autoFlip.controledBook.EndBottomLeft.x) * 0.5f) * 1f;
			this.h = Mathf.Abs(autoFlip.controledBook.EndBottomRight.y) * 1f;
			this.dx = (this.xl) * 2 / autoFlip.animationFramesCount;
		}
	}

    void Start () {
		if (!controledBook) {
			controledBook = GetComponent<Book>();
		}

		multiplier = new Multiplier (this);

		if (autoStartFlip) {
			FlipOnceByMode (mode);
		}
	
		controledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
	}

    void PageFlipped() {
        isFlipping = false;
    }

	public void StartFlipping() {
        StartCoroutine(FlipToEnd());
    }

    public void FlipRightPage() {
		if (isFlipping || controledBook.currentPage >= controledBook.TotalPageCount) { 
			return;
		}
        
        isFlipping = true;

		StartCoroutine(FlipRTL(multiplier));
    }

    public void FlipLeftPage() {
        if (isFlipping) return;
        if (controledBook.currentPage <= 0) return;
        isFlipping = true;

		StartCoroutine(FlipLTR(multiplier));
    }

	private void FlipOnceByMode(FlipMode Mode) {
		switch (Mode) {
		case FlipMode.RightToLeft:
			StartCoroutine(FlipRTL(multiplier));
			break;
		case FlipMode.LeftToRight:
			StartCoroutine(FlipLTR(multiplier));
			break;
		}
	}

    IEnumerator FlipToEnd() {
        yield return new WaitForSeconds(delayBeforeStarting);

        switch (mode) {
            case FlipMode.RightToLeft:
                while (controledBook.currentPage < controledBook.TotalPageCount) {
					StartCoroutine(FlipRTL(multiplier));
                    yield return new WaitForSeconds(timeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (controledBook.currentPage > 0) {
					StartCoroutine(FlipLTR(multiplier));
                    yield return new WaitForSeconds(timeBetweenPages);
                }
                break;
        }
    }

	IEnumerator FlipRTL(Multiplier m) {
		float x = m.xc + m.xl;
		float y = (-m.h / (m.xl * m.xl)) * (x - m.xc) * (x - m.xc);

        controledBook.DragRightPageToPoint(new Vector3(x, y, 0));

        for (int i = 0; i < animationFramesCount; i++) {
            y = (-m.h / (m.xl * m.xl)) * (x - m.xc) * (x - m.xc);
            
			controledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            
			yield return new WaitForSeconds(m.frameTime);
            
			x -= m.dx;
        }

        controledBook.ReleasePage();
    }

	IEnumerator FlipLTR(Multiplier m) {
		float x = m.xc - m.xl;
		float y = (-m.h / (m.xl * m.xl)) * (x - m.xc) * (x - m.xc);
        
		controledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        
		for (int i = 0; i < animationFramesCount; i++) {
			y = (-m.h / (m.xl * m.xl)) * (x - m.xc) * (x - m.xc);
            
			controledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            
			yield return new WaitForSeconds(m.frameTime);

			x += m.dx;
        }

        controledBook.ReleasePage();
    }
}