public class ActorHealthEventArgs
{
    public IActor Actor { get; set; }
    public float Amount { get; set; }
    public bool IsDamage { get; set; }
}