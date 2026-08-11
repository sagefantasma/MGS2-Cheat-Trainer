using System.Collections.Generic;

namespace MGS2_CheatTrainer_V2.Models.PlayableCharacters
{
    public abstract class PlayableCharacter
    {
        public abstract IReadOnlyList<Constants.IMgs2Object> ObjectList { get; }
    }
}
