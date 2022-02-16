using System.ComponentModel;

namespace AngelusTBlog.Enums
{
    public enum ModerationType
    {
        [Description("Political Properganda")]
        PoliticalProperganda,
        [Description("Obscene Language")]
        ObsceneLanguage,
        [Description("Drugs References")]
        DrugsReferences,
        [Description("Religious Speach")]
        ReligiousSpeach,
        [Description("Threatening Speach")]
        ThreateningSpeach,
        [Description("Sexual Content")]
        SexualContent,
        [Description("Hate Speach")]
        HateSpeach,
        [Description("Targeted Shaming")]
        TargetedShaming
    }
}
