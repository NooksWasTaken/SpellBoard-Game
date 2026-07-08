public interface IPuzzle
{
    PuzzleResult TrySolve(PlayerSpells spell);
    PlayerSpells GetCurrentRequiredSpell();

    void Select();

    void Deselect();

    float GetTimeLimit();
}