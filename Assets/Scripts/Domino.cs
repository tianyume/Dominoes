public class Domino {
    // Value1 is always less than or equal to Value2
    public int Value1 { get; private set; }
    public int Value2 { get; private set; }

    public bool IsObservableByAll { get; set; }
    public DominoDirection IsHorizontal { get; set; }
    public GameRole Ownership { get; set; }

    Domino(int value1, int value2, bool isObservableByAll, DominoDirection isHorizontal, GameRole ownership)
    {
        Value1 = value1;
        Value2 = value2;
        IsObservableByAll = isObservableByAll;
        IsHorizontal = isHorizontal;
        Ownership = ownership;
    }
}
