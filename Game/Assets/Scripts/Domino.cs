public class Domino {
    // Value1 is always less than or equal to Value2
    public int Value1 { get; private set; }
    public int Value2 { get; private set; }

    public bool IsObservableByAll { get; set; }
    public GameRole Ownership { get; set; }
	public DominoPlacement Placement;

    public Domino(int value1, int value2)
    {
        Value1 = value1;
        Value2 = value2;
		IsObservableByAll = false;
		Ownership = GameRole.BoneYard;
		Placement.Direction = DominoDirection.NotSpecified;
		Placement.LeftValue = -1;
		Placement.RightValue = -1;
		Placement.UpValue = -1;
		Placement.DownValue = -1;
    }
}
