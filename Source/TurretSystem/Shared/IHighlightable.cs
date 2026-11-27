public interface IHighlightable
{
    void OnHighlight();
    void OnHighlightLost();

    HighlightType GetHighlightType();
}
