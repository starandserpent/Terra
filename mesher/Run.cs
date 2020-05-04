public struct Run{
    public int lenght{get; set;}
    public int value{get; set;}
    public static bool operator ==(Run runA, Run runB){
        return runA.value == runB.value;
    }

    public static bool operator !=(Run runA, Run runB){
        return runA.value != runB.value;
    }
}