public class CharacterMakeupData
{
    public bool cream = false;
    public MakeupColorId blushColorId = MakeupColorId.None;
    public MakeupColorId eyeShadowColorId = MakeupColorId.None;
    public MakeupColorId lipstickColorId = MakeupColorId.None;

    public void SetData(MakeupType makeupType, MakeupColorId makeupColorId)
    {
        switch (makeupType)
        {
            case MakeupType.Blush:
                blushColorId = makeupColorId;
                break;
            case MakeupType.Eyeshadow: 
                eyeShadowColorId = makeupColorId;
                break;
            case MakeupType.Lipstick:
                lipstickColorId = makeupColorId;
                break;
            case MakeupType.Cream:
                cream = true;
                break;
        }
    }

    public void Reset()
    {
        cream = false;
        blushColorId = MakeupColorId.None;
        eyeShadowColorId = MakeupColorId.None;
        lipstickColorId = MakeupColorId.None;
    }
}
