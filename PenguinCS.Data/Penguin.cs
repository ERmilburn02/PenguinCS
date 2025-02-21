using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// Penguins
/// </summary>
public partial class Penguin
{
    /// <summary>
    /// Unique penguin ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Penguin login name
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Penguin display name
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// Password hash
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// User Email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Date of registration
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// email activated
    /// </summary>
    public bool Active { get; set; }

    public bool SafeChat { get; set; }

    /// <summary>
    /// EPF previous paycheck
    /// </summary>
    public DateTime LastPaycheck { get; set; }

    /// <summary>
    /// Total minutes connected
    /// </summary>
    public int MinutesPlayed { get; set; }

    /// <summary>
    /// Is user moderator?
    /// </summary>
    public bool Moderator { get; set; }

    public bool StealthModerator { get; set; }

    /// <summary>
    /// Character ID
    /// </summary>
    public int? Character { get; set; }

    /// <summary>
    /// Penguin active igloo ID
    /// </summary>
    public int? Igloo { get; set; }

    /// <summary>
    /// Penguin coins
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// Penguin color ID
    /// </summary>
    public int? Color { get; set; }

    /// <summary>
    /// Penguin head item ID
    /// </summary>
    public int? Head { get; set; }

    /// <summary>
    /// Penguin face item ID
    /// </summary>
    public int? Face { get; set; }

    /// <summary>
    /// Penguin neck item ID
    /// </summary>
    public int? Neck { get; set; }

    /// <summary>
    /// Penguin body item ID
    /// </summary>
    public int? Body { get; set; }

    /// <summary>
    /// Penguin hand item ID
    /// </summary>
    public int? Hand { get; set; }

    /// <summary>
    /// Penguin feet item ID
    /// </summary>
    public int? Feet { get; set; }

    /// <summary>
    /// Penguin background ID
    /// </summary>
    public int? Photo { get; set; }

    /// <summary>
    /// Penguin pin ID
    /// </summary>
    public int? Flag { get; set; }

    /// <summary>
    /// Is penguin banned forever?
    /// </summary>
    public bool Permaban { get; set; }

    /// <summary>
    /// Is book cover modified?
    /// </summary>
    public int BookModified { get; set; }

    /// <summary>
    /// Stampbook cover color
    /// </summary>
    public int BookColor { get; set; }

    /// <summary>
    /// Stampbook highlight color
    /// </summary>
    public int BookHighlight { get; set; }

    /// <summary>
    /// Stampbook cover pattern
    /// </summary>
    public int BookPattern { get; set; }

    /// <summary>
    /// Stampbook cover icon
    /// </summary>
    public int BookIcon { get; set; }

    /// <summary>
    /// Is penguin EPF agent?
    /// </summary>
    public bool AgentStatus { get; set; }

    /// <summary>
    /// Is field op complete?
    /// </summary>
    public int FieldOpStatus { get; set; }

    /// <summary>
    /// Total career medals
    /// </summary>
    public int CareerMedals { get; set; }

    /// <summary>
    /// Current medals
    /// </summary>
    public int AgentMedals { get; set; }

    /// <summary>
    /// Date of last field op
    /// </summary>
    public DateTime LastFieldOp { get; set; }

    /// <summary>
    /// Recent agent message read
    /// </summary>
    public DateTime ComMessageReadDate { get; set; }

    /// <summary>
    /// Ninja rank
    /// </summary>
    public int NinjaRank { get; set; }

    /// <summary>
    /// Ninja progress
    /// </summary>
    public int NinjaProgress { get; set; }

    /// <summary>
    /// Fire ninja rank
    /// </summary>
    public int FireNinjaRank { get; set; }

    /// <summary>
    /// Fire ninja progress
    /// </summary>
    public int FireNinjaProgress { get; set; }

    /// <summary>
    /// Water ninja rank
    /// </summary>
    public int WaterNinjaRank { get; set; }

    /// <summary>
    /// Water ninja progress
    /// </summary>
    public int WaterNinjaProgress { get; set; }

    public int SnowNinjaRank { get; set; }

    public int SnowNinjaProgress { get; set; }

    /// <summary>
    /// CardJitsu matches won
    /// </summary>
    public int NinjaMatchesWon { get; set; }

    /// <summary>
    /// JitsuFire matches won
    /// </summary>
    public int FireMatchesWon { get; set; }

    /// <summary>
    /// JitsuWater matces won
    /// </summary>
    public int WaterMatchesWon { get; set; }

    public int SnowProgressFireWins { get; set; }

    public int SnowProgressWaterWins { get; set; }

    public int SnowProgressSnowWins { get; set; }

    /// <summary>
    /// Rainbow puffle adoptability status
    /// </summary>
    public bool RainbowAdoptability { get; set; }

    /// <summary>
    /// Puffle digging boolean
    /// </summary>
    public bool HasDug { get; set; }

    /// <summary>
    /// Has met puffle handler?
    /// </summary>
    public bool PuffleHandler { get; set; }

    /// <summary>
    /// Golden puffle nuggets
    /// </summary>
    public int Nuggets { get; set; }

    /// <summary>
    /// Walking puffle ID
    /// </summary>
    public int? Walking { get; set; }

    /// <summary>
    /// Has player opened playercard?
    /// </summary>
    public bool OpenedPlayercard { get; set; }

    public bool SpecialWave { get; set; }

    public bool SpecialDance { get; set; }

    public bool SpecialSnowball { get; set; }

    /// <summary>
    /// Currently selected map category
    /// </summary>
    public int MapCategory { get; set; }

    /// <summary>
    /// New player status field
    /// </summary>
    public int StatusField { get; set; }

    /// <summary>
    /// Is egg-timer active?
    /// </summary>
    public bool TimerActive { get; set; }

    /// <summary>
    /// Egg-timer start time
    /// </summary>
    public TimeOnly TimerStart { get; set; }

    /// <summary>
    /// Egg-timer end time
    /// </summary>
    public TimeOnly TimerEnd { get; set; }

    /// <summary>
    /// Egg-timer total play time
    /// </summary>
    public TimeSpan TimerTotal { get; set; }

    /// <summary>
    /// Is player grounded?
    /// </summary>
    public bool Grounded { get; set; }

    /// <summary>
    /// English username approval
    /// </summary>
    public bool ApprovalEn { get; set; }

    /// <summary>
    /// Portuguese username approval
    /// </summary>
    public bool ApprovalPt { get; set; }

    /// <summary>
    /// French username approval
    /// </summary>
    public bool ApprovalFr { get; set; }

    /// <summary>
    /// Spanish username approval
    /// </summary>
    public bool ApprovalEs { get; set; }

    /// <summary>
    /// Dutch username approval
    /// </summary>
    public bool ApprovalDe { get; set; }

    /// <summary>
    /// Russian username approval
    /// </summary>
    public bool ApprovalRu { get; set; }

    public bool RejectionEn { get; set; }

    public bool RejectionPt { get; set; }

    public bool RejectionFr { get; set; }

    public bool RejectionEs { get; set; }

    public bool RejectionDe { get; set; }

    public bool RejectionRu { get; set; }

    public virtual ICollection<ActivationKey> ActivationKeys { get; set; } = new List<ActivationKey>();

    public virtual ICollection<Ban> BanModerators { get; set; } = new List<Ban>();

    public virtual ICollection<Ban> BanPenguins { get; set; } = new List<Ban>();

    public virtual Item BodyNavigation { get; set; }

    public virtual ICollection<BuddyList> BuddyListBuddies { get; set; } = new List<BuddyList>();

    public virtual ICollection<BuddyList> BuddyListPenguins { get; set; } = new List<BuddyList>();

    public virtual ICollection<CfcDonation> CfcDonations { get; set; } = new List<CfcDonation>();

    public virtual ICollection<CharacterBuddy> CharacterBuddies { get; set; } = new List<CharacterBuddy>();

    public virtual Character CharacterNavigation { get; set; }

    public virtual Item ColorNavigation { get; set; }

    public virtual ICollection<CoverItem> CoverItems { get; set; } = new List<CoverItem>();

    public virtual ICollection<CoverStamp> CoverStamps { get; set; } = new List<CoverStamp>();

    public virtual Item FaceNavigation { get; set; }

    public virtual Item FeetNavigation { get; set; }

    public virtual Item FlagNavigation { get; set; }

    public virtual Item HandNavigation { get; set; }

    public virtual Item HeadNavigation { get; set; }

    public virtual ICollection<IglooLike> IglooLikes { get; set; } = new List<IglooLike>();

    public virtual PenguinIglooRoom IglooNavigation { get; set; }

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual Item NeckNavigation { get; set; }

    public virtual ICollection<PenguinAttribute> PenguinAttributes { get; set; } = new List<PenguinAttribute>();

    public virtual ICollection<PenguinCard> PenguinCards { get; set; } = new List<PenguinCard>();

    public virtual ICollection<PenguinFurniture> PenguinFurnitures { get; set; } = new List<PenguinFurniture>();

    public virtual ICollection<PenguinGameDatum> PenguinGameData { get; set; } = new List<PenguinGameDatum>();

    public virtual ICollection<PenguinIglooRoom> PenguinIglooRooms { get; set; } = new List<PenguinIglooRoom>();

    public virtual ICollection<PenguinMembership> PenguinMemberships { get; set; } = new List<PenguinMembership>();

    public virtual ICollection<PenguinPostcard> PenguinPostcardPenguins { get; set; } = new List<PenguinPostcard>();

    public virtual ICollection<PenguinPostcard> PenguinPostcardSenders { get; set; } = new List<PenguinPostcard>();

    public virtual ICollection<PenguinPuffleItem> PenguinPuffleItems { get; set; } = new List<PenguinPuffleItem>();

    public virtual ICollection<PenguinPuffle> PenguinPuffles { get; set; } = new List<PenguinPuffle>();

    public virtual ICollection<PenguinQuestTask> PenguinQuestTasks { get; set; } = new List<PenguinQuestTask>();

    public virtual ICollection<PenguinStamp> PenguinStamps { get; set; } = new List<PenguinStamp>();

    public virtual ICollection<PenguinTrack> PenguinTracks { get; set; } = new List<PenguinTrack>();

    public virtual Item PhotoNavigation { get; set; }

    public virtual ICollection<Report> ReportPenguins { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportReporters { get; set; } = new List<Report>();

    public virtual ICollection<TrackLike> TrackLikes { get; set; } = new List<TrackLike>();

    public virtual PenguinPuffle WalkingNavigation { get; set; }

    public virtual ICollection<Warning> Warnings { get; set; } = new List<Warning>();

    public virtual ICollection<RedemptionBook> Books { get; set; } = new List<RedemptionBook>();

    public virtual ICollection<RedemptionCode> Codes { get; set; } = new List<RedemptionCode>();

    public virtual ICollection<Flooring> Floorings { get; set; } = new List<Flooring>();

    public virtual ICollection<Igloo> Igloos { get; set; } = new List<Igloo>();

    public virtual ICollection<Penguin> Ignores { get; set; } = new List<Penguin>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<Penguin> Penguins { get; set; } = new List<Penguin>();

    public virtual ICollection<Penguin> PenguinsNavigation { get; set; } = new List<Penguin>();

    public virtual ICollection<Permission> PermissionNames { get; set; } = new List<Permission>();

    public virtual ICollection<Penguin> Requesters { get; set; } = new List<Penguin>();
}
