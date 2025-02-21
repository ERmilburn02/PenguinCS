using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PenguinCS.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Database.EnsureCreated();
        }

        public virtual DbSet<ActivationKey> ActivationKeys { get; set; }

        public virtual DbSet<Ban> Bans { get; set; }

        public virtual DbSet<BuddyList> BuddyLists { get; set; }

        public virtual DbSet<Card> Cards { get; set; }

        public virtual DbSet<CardStarterDeck> CardStarterDecks { get; set; }

        public virtual DbSet<CfcDonation> CfcDonations { get; set; }

        public virtual DbSet<Character> Characters { get; set; }

        public virtual DbSet<CharacterBuddy> CharacterBuddies { get; set; }

        public virtual DbSet<ChatFilterRule> ChatFilterRules { get; set; }

        public virtual DbSet<CoverItem> CoverItems { get; set; }

        public virtual DbSet<CoverStamp> CoverStamps { get; set; }

        public virtual DbSet<DanceSong> DanceSongs { get; set; }

        public virtual DbSet<EpfComMessage> EpfComMessages { get; set; }

        public virtual DbSet<Flooring> Floorings { get; set; }

        public virtual DbSet<Furniture> Furnitures { get; set; }

        public virtual DbSet<Igloo> Igloos { get; set; }

        public virtual DbSet<IglooFurniture> IglooFurnitures { get; set; }

        public virtual DbSet<IglooLike> IglooLikes { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Login> Logins { get; set; }

        public virtual DbSet<Penguin> Penguins { get; set; }

        public virtual DbSet<PenguinAttribute> PenguinAttributes { get; set; }

        public virtual DbSet<PenguinCard> PenguinCards { get; set; }

        public virtual DbSet<PenguinFurniture> PenguinFurnitures { get; set; }

        public virtual DbSet<PenguinGameDatum> PenguinGameData { get; set; }

        public virtual DbSet<PenguinIglooRoom> PenguinIglooRooms { get; set; }

        public virtual DbSet<PenguinMembership> PenguinMemberships { get; set; }

        public virtual DbSet<PenguinPostcard> PenguinPostcards { get; set; }

        public virtual DbSet<PenguinPuffle> PenguinPuffles { get; set; }

        public virtual DbSet<PenguinPuffleItem> PenguinPuffleItems { get; set; }

        public virtual DbSet<PenguinQuestTask> PenguinQuestTasks { get; set; }

        public virtual DbSet<PenguinStamp> PenguinStamps { get; set; }

        public virtual DbSet<PenguinTrack> PenguinTracks { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<PluginAttribute> PluginAttributes { get; set; }

        public virtual DbSet<Postcard> Postcards { get; set; }

        public virtual DbSet<Puffle> Puffles { get; set; }

        public virtual DbSet<PuffleItem> PuffleItems { get; set; }

        public virtual DbSet<Quest> Quests { get; set; }

        public virtual DbSet<QuestAwardFurniture> QuestAwardFurnitures { get; set; }

        public virtual DbSet<QuestAwardPuffleItem> QuestAwardPuffleItems { get; set; }

        public virtual DbSet<QuestTask> QuestTasks { get; set; }

        public virtual DbSet<RedemptionBook> RedemptionBooks { get; set; }

        public virtual DbSet<RedemptionBookWord> RedemptionBookWords { get; set; }

        public virtual DbSet<RedemptionCode> RedemptionCodes { get; set; }

        public virtual DbSet<Report> Reports { get; set; }

        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<RoomTable> RoomTables { get; set; }

        public virtual DbSet<RoomWaddle> RoomWaddles { get; set; }

        public virtual DbSet<Stamp> Stamps { get; set; }

        public virtual DbSet<StampGroup> StampGroups { get; set; }

        public virtual DbSet<TrackLike> TrackLikes { get; set; }

        public virtual DbSet<Warning> Warnings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivationKey>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.ActivationKey1 }).HasName("activation_key_pkey");

                entity.ToTable("activation_key", tb => tb.HasComment("Penguin activation keys"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.ActivationKey1)
                    .HasMaxLength(255)
                    .IsFixedLength()
                    .HasComment("Penguin activation key")
                    .HasColumnName("activation_key");

                entity.HasOne(d => d.Penguin).WithMany(p => p.ActivationKeys)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("activation_key_ibfk_1");
            });

            modelBuilder.Entity<Ban>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.Issued, e.Expires }).HasName("ban_pkey");

                entity.ToTable("ban", tb => tb.HasComment("Penguin ban records"));

                entity.HasIndex(e => e.ModeratorId, "ban_moderator_id");

                entity.Property(e => e.PenguinId)
                    .HasComment("Banned penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Issued)
                    .HasDefaultValueSql("now()")
                    .HasComment("Issue date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("issued");
                entity.Property(e => e.Expires)
                    .HasDefaultValueSql("now()")
                    .HasComment("Expiry date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("expires");
                entity.Property(e => e.Comment)
                    .HasComment("Ban comment")
                    .HasColumnName("comment");
                entity.Property(e => e.Message)
                    .HasComment("Banned for message")
                    .HasColumnName("message");
                entity.Property(e => e.ModeratorId)
                    .HasComment("Moderator penguin ID")
                    .HasColumnName("moderator_id");
                entity.Property(e => e.Reason)
                    .HasComment("Ban reason")
                    .HasColumnName("reason");

                entity.HasOne(d => d.Moderator).WithMany(p => p.BanModerators)
                    .HasForeignKey(d => d.ModeratorId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ban_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.BanPenguins)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("ban_ibfk_1");
            });

            modelBuilder.Entity<BuddyList>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.BuddyId }).HasName("buddy_list_pkey");

                entity.ToTable("buddy_list", tb => tb.HasComment("Penguin buddy relationships"));

                entity.HasIndex(e => e.BuddyId, "buddy_id");

                entity.Property(e => e.PenguinId).HasColumnName("penguin_id");
                entity.Property(e => e.BuddyId).HasColumnName("buddy_id");
                entity.Property(e => e.BestBuddy)
                    .HasDefaultValue(false)
                    .HasColumnName("best_buddy");

                entity.HasOne(d => d.Buddy).WithMany(p => p.BuddyListBuddies)
                    .HasForeignKey(d => d.BuddyId)
                    .HasConstraintName("buddy_list_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.BuddyListPenguins)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("buddy_list_ibfk_1");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("card_pkey");

                entity.ToTable("card", tb => tb.HasComment("Server jitsu card crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique card ID")
                    .HasColumnName("id");
                entity.Property(e => e.Color)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("'b'::bpchar")
                    .HasComment("Card color")
                    .HasColumnName("color");
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Play description")
                    .HasColumnName("description");
                entity.Property(e => e.Element)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("'s'::bpchar")
                    .HasComment("Card element")
                    .HasColumnName("element");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Card name")
                    .HasColumnName("name");
                entity.Property(e => e.PowerId)
                    .HasDefaultValue(0)
                    .HasComment("Card power ID")
                    .HasColumnName("power_id");
                entity.Property(e => e.SetId)
                    .HasDefaultValue(1)
                    .HasComment("Card set ID")
                    .HasColumnName("set_id");
                entity.Property(e => e.Value)
                    .HasDefaultValue(2)
                    .HasComment("Value of card")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<CardStarterDeck>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.CardId }).HasName("card_starter_deck_pkey");

                entity.ToTable("card_starter_deck", tb => tb.HasComment("Jitsu card starter decks"));

                entity.Property(e => e.ItemId)
                    .HasComment("Starter deck item ID")
                    .HasColumnName("item_id");
                entity.Property(e => e.CardId)
                    .HasComment("Starter deck card ID")
                    .HasColumnName("card_id");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasComment("Card quantity")
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Card).WithMany(p => p.CardStarterDecks)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("card_starter_deck_ibfk_2");

                entity.HasOne(d => d.Item).WithMany(p => p.CardStarterDecks)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("card_starter_deck_ibfk_1");
            });

            modelBuilder.Entity<CfcDonation>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.Charity, e.Date }).HasName("cfc_donation_pkey");

                entity.ToTable("cfc_donation", tb => tb.HasComment("CFC charity donations"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Donator penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Charity)
                    .HasComment("Donation charity or cause")
                    .HasColumnName("charity");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of donation")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");
                entity.Property(e => e.Coins)
                    .HasComment("Donation coin amount")
                    .HasColumnName("coins");

                entity.HasOne(d => d.Penguin).WithMany(p => p.CfcDonations)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("cfc_donation_ibfk_1");
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("character_pkey");

                entity.ToTable("character", tb => tb.HasComment("Server character crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique character ID")
                    .HasColumnName("id");
                entity.Property(e => e.GiftId)
                    .HasComment("Character gift item ID")
                    .HasColumnName("gift_id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Character name")
                    .HasColumnName("name");
                entity.Property(e => e.StampId)
                    .HasComment("Character stamp ID")
                    .HasColumnName("stamp_id");

                entity.HasOne(d => d.Gift).WithMany(p => p.Characters)
                    .HasForeignKey(d => d.GiftId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("character_ibfk_1");

                entity.HasOne(d => d.Stamp).WithMany(p => p.Characters)
                    .HasForeignKey(d => d.StampId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("character_ibfk_2");
            });

            modelBuilder.Entity<CharacterBuddy>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.CharacterId }).HasName("character_buddy_pkey");

                entity.ToTable("character_buddy", tb => tb.HasComment("Penguin character buddies"));

                entity.Property(e => e.PenguinId).HasColumnName("penguin_id");
                entity.Property(e => e.CharacterId).HasColumnName("character_id");
                entity.Property(e => e.BestBuddy)
                    .HasDefaultValue(false)
                    .HasColumnName("best_buddy");

                entity.HasOne(d => d.Character).WithMany(p => p.CharacterBuddies)
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("character_buddy_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.CharacterBuddies)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("character_buddy_ibfk_1");
            });

            modelBuilder.Entity<ChatFilterRule>(entity =>
            {
                entity.HasKey(e => e.Word).HasName("chat_filter_rule_pkey");

                entity.ToTable("chat_filter_rule");

                entity.HasIndex(e => e.Word, "chat_filter_rule_word").IsUnique();

                entity.Property(e => e.Word)
                    .HasComment("Word to filter")
                    .HasColumnName("word");
                entity.Property(e => e.Ban)
                    .HasDefaultValue(false)
                    .HasComment("Ban player for word")
                    .HasColumnName("ban");
                entity.Property(e => e.Filter)
                    .HasDefaultValue(false)
                    .HasComment("Hide word from players")
                    .HasColumnName("filter");
                entity.Property(e => e.Warn)
                    .HasDefaultValue(false)
                    .HasComment("Warn player for word")
                    .HasColumnName("warn");
            });

            modelBuilder.Entity<CoverItem>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.ItemId }).HasName("cover_item_pkey");

                entity.ToTable("cover_item", tb => tb.HasComment("Items placed on book cover"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Unique penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.ItemId)
                    .HasComment("Cover item ID")
                    .HasColumnName("item_id");
                entity.Property(e => e.Depth)
                    .HasDefaultValue(0)
                    .HasComment("Stamp cover depth")
                    .HasColumnName("depth");
                entity.Property(e => e.Rotation)
                    .HasDefaultValue(0)
                    .HasComment("Stamp cover rotation")
                    .HasColumnName("rotation");
                entity.Property(e => e.X)
                    .HasDefaultValue(0)
                    .HasComment("Cover X position")
                    .HasColumnName("x");
                entity.Property(e => e.Y)
                    .HasDefaultValue(0)
                    .HasComment("Cover Y position")
                    .HasColumnName("y");

                entity.HasOne(d => d.Item).WithMany(p => p.CoverItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("cover_item_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.CoverItems)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("cover_item_ibfk_1");
            });

            modelBuilder.Entity<CoverStamp>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.StampId }).HasName("cover_stamp_pkey");

                entity.ToTable("cover_stamp", tb => tb.HasComment("Stamps placed on book cover"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Unique penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.StampId)
                    .HasComment("Cover stamp ID")
                    .HasColumnName("stamp_id");
                entity.Property(e => e.Depth)
                    .HasDefaultValue(0)
                    .HasComment("Stamp cover depth")
                    .HasColumnName("depth");
                entity.Property(e => e.Rotation)
                    .HasDefaultValue(0)
                    .HasComment("Stamp cover rotation")
                    .HasColumnName("rotation");
                entity.Property(e => e.X)
                    .HasDefaultValue(0)
                    .HasComment("Cover X position")
                    .HasColumnName("x");
                entity.Property(e => e.Y)
                    .HasDefaultValue(0)
                    .HasComment("Cover Y position")
                    .HasColumnName("y");

                entity.HasOne(d => d.Penguin).WithMany(p => p.CoverStamps)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("cover_stamp_ibfk_1");

                entity.HasOne(d => d.Stamp).WithMany(p => p.CoverStamps)
                    .HasForeignKey(d => d.StampId)
                    .HasConstraintName("cover_stamp_ibfk_2");
            });

            modelBuilder.Entity<DanceSong>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("dance_song_pkey");

                entity.ToTable("dance_song", tb => tb.HasComment("Dance contest multiplayer tracks"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique song ID")
                    .HasColumnName("id");
                entity.Property(e => e.MillisPerBar)
                    .HasComment("Milliseconds per song note")
                    .HasColumnName("millis_per_bar");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Name of song")
                    .HasColumnName("name");
                entity.Property(e => e.SongLength)
                    .HasComment("Length of song in beats")
                    .HasColumnName("song_length");
                entity.Property(e => e.SongLengthMillis)
                    .HasComment("Length of song in milliseconds")
                    .HasColumnName("song_length_millis");
            });

            modelBuilder.Entity<EpfComMessage>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("epf_com_message", tb => tb.HasComment("EPF phone com messages"));

                entity.Property(e => e.CharacterId)
                    .HasComment("Character ID of message")
                    .HasColumnName("character_id");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of message creation")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");
                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasComment("Message content")
                    .HasColumnName("message");

                entity.HasOne(d => d.Character).WithMany()
                    .HasForeignKey(d => d.CharacterId)
                    .HasConstraintName("epf_com_message_ibfk_1");
            });

            modelBuilder.Entity<Flooring>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("flooring_pkey");

                entity.ToTable("flooring", tb => tb.HasComment("Server flooring crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique flooring ID")
                    .HasColumnName("id");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of flooring")
                    .HasColumnName("cost");
                entity.Property(e => e.LegacyInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default legacy inventory?")
                    .HasColumnName("legacy_inventory");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Flooring name")
                    .HasColumnName("name");
                entity.Property(e => e.Patched)
                    .HasDefaultValue(false)
                    .HasComment("Is flooring patched?")
                    .HasColumnName("patched");
                entity.Property(e => e.VanillaInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default vanilla inventory?")
                    .HasColumnName("vanilla_inventory");
            });

            modelBuilder.Entity<Furniture>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("furniture_pkey");

                entity.ToTable("furniture", tb => tb.HasComment("Server furniture crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique furniture ID")
                    .HasColumnName("id");
                entity.Property(e => e.Bait)
                    .HasDefaultValue(false)
                    .HasComment("Is furniture bait?")
                    .HasColumnName("bait");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of furniture")
                    .HasColumnName("cost");
                entity.Property(e => e.Innocent)
                    .HasDefaultValue(false)
                    .HasComment("Is furniture innocent?")
                    .HasColumnName("innocent");
                entity.Property(e => e.LegacyInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default legacy inventory?")
                    .HasColumnName("legacy_inventory");
                entity.Property(e => e.MaxQuantity)
                    .HasDefaultValue(100)
                    .HasComment("Max inventory quantity")
                    .HasColumnName("max_quantity");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.Patched)
                    .HasDefaultValue(false)
                    .HasComment("Is furniture patched?")
                    .HasColumnName("patched");
                entity.Property(e => e.Sort)
                    .HasDefaultValue(1)
                    .HasComment("Furniture sort ID")
                    .HasColumnName("sort");
                entity.Property(e => e.Type)
                    .HasDefaultValue(1)
                    .HasComment("Furniture type ID")
                    .HasColumnName("type");
                entity.Property(e => e.VanillaInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default vanilla inventory?")
                    .HasColumnName("vanilla_inventory");
            });

            modelBuilder.Entity<Igloo>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("igloo_pkey");

                entity.ToTable("igloo", tb => tb.HasComment("Server igloo crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique igloo ID")
                    .HasColumnName("id");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of igloo")
                    .HasColumnName("cost");
                entity.Property(e => e.LegacyInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default legacy inventory?")
                    .HasColumnName("legacy_inventory");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Igloo name")
                    .HasColumnName("name");
                entity.Property(e => e.Patched)
                    .HasDefaultValue(false)
                    .HasComment("Is igloo patched?")
                    .HasColumnName("patched");
                entity.Property(e => e.VanillaInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default vanilla inventory?")
                    .HasColumnName("vanilla_inventory");
            });

            modelBuilder.Entity<IglooFurniture>(entity =>
            {
                entity.HasKey(e => new { e.IglooId, e.FurnitureId, e.X, e.Y, e.Frame, e.Rotation }).HasName("igloo_furniture_pkey");

                entity.ToTable("igloo_furniture", tb => tb.HasComment("Furniture placed inside igloos"));

                entity.HasIndex(e => e.IglooId, "igloo_furniture_igloo_id");

                entity.Property(e => e.IglooId)
                    .HasComment("Furniture igloo ID")
                    .HasColumnName("igloo_id");
                entity.Property(e => e.FurnitureId)
                    .HasComment("Furniture item ID")
                    .HasColumnName("furniture_id");
                entity.Property(e => e.X)
                    .HasDefaultValue(0)
                    .HasComment("Igloo X position")
                    .HasColumnName("x");
                entity.Property(e => e.Y)
                    .HasDefaultValue(0)
                    .HasComment("Igloo Y position")
                    .HasColumnName("y");
                entity.Property(e => e.Frame)
                    .HasDefaultValue(0)
                    .HasComment("Furniture frame ID")
                    .HasColumnName("frame");
                entity.Property(e => e.Rotation)
                    .HasDefaultValue(0)
                    .HasComment("Furniture rotation ID")
                    .HasColumnName("rotation");

                entity.HasOne(d => d.Furniture).WithMany(p => p.IglooFurnitures)
                    .HasForeignKey(d => d.FurnitureId)
                    .HasConstraintName("igloo_furniture_ibfk_2");

                entity.HasOne(d => d.Igloo).WithMany(p => p.IglooFurnitures)
                    .HasForeignKey(d => d.IglooId)
                    .HasConstraintName("igloo_furniture_ibfk_1");
            });

            modelBuilder.Entity<IglooLike>(entity =>
            {
                entity.HasKey(e => new { e.IglooId, e.PlayerId }).HasName("igloo_like_pkey");

                entity.ToTable("igloo_like", tb => tb.HasComment("Player igloo likes"));

                entity.Property(e => e.IglooId)
                    .HasComment("Igloo unique ID")
                    .HasColumnName("igloo_id");
                entity.Property(e => e.PlayerId)
                    .HasComment("Liker unique ID")
                    .HasColumnName("player_id");
                entity.Property(e => e.Count)
                    .HasDefaultValue(1)
                    .HasComment("Number of likes")
                    .HasColumnName("count");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of like")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.HasOne(d => d.Igloo).WithMany(p => p.IglooLikes)
                    .HasForeignKey(d => d.IglooId)
                    .HasConstraintName("igloo_like_ibfk_1");

                entity.HasOne(d => d.Player).WithMany(p => p.IglooLikes)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("igloo_like_ibfk_2");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("item_pkey");

                entity.ToTable("item", tb => tb.HasComment("Server item crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique item ID")
                    .HasColumnName("id");
                entity.Property(e => e.Bait)
                    .HasDefaultValue(false)
                    .HasComment("Is bait item?")
                    .HasColumnName("bait");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of item")
                    .HasColumnName("cost");
                entity.Property(e => e.Epf)
                    .HasDefaultValue(false)
                    .HasComment("Is EPF item?")
                    .HasColumnName("epf");
                entity.Property(e => e.Innocent)
                    .HasDefaultValue(false)
                    .HasComment("Is a innocent item?")
                    .HasColumnName("innocent");
                entity.Property(e => e.LegacyInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default legacy inventory?")
                    .HasColumnName("legacy_inventory");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Item name")
                    .HasColumnName("name");
                entity.Property(e => e.Patched)
                    .HasDefaultValue(false)
                    .HasComment("Is item patched?")
                    .HasColumnName("patched");
                entity.Property(e => e.ReleaseDate)
                    .HasDefaultValueSql("now()")
                    .HasComment("Release date of item")
                    .HasColumnName("release_date");
                entity.Property(e => e.Tour)
                    .HasDefaultValue(false)
                    .HasComment("Gives tour status?")
                    .HasColumnName("tour");
                entity.Property(e => e.Treasure)
                    .HasDefaultValue(false)
                    .HasComment("Is a treasure item?")
                    .HasColumnName("treasure");
                entity.Property(e => e.Type)
                    .HasDefaultValue(1)
                    .HasComment("Item clothing type")
                    .HasColumnName("type");
                entity.Property(e => e.VanillaInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default vanilla inventory?")
                    .HasColumnName("vanilla_inventory");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("location_pkey");

                entity.ToTable("location", tb => tb.HasComment("Server location crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique location ID")
                    .HasColumnName("id");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of location")
                    .HasColumnName("cost");
                entity.Property(e => e.LegacyInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default legacy inventory?")
                    .HasColumnName("legacy_inventory");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Location name")
                    .HasColumnName("name");
                entity.Property(e => e.Patched)
                    .HasDefaultValue(false)
                    .HasComment("Is location patched?")
                    .HasColumnName("patched");
                entity.Property(e => e.VanillaInventory)
                    .HasDefaultValue(false)
                    .HasComment("Add to default vanilla inventory?")
                    .HasColumnName("vanilla_inventory");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("login_pkey");

                entity.ToTable("login", tb => tb.HasComment("Penguin login records"));

                entity.Property(e => e.Id)
                    .HasComment("Unique login ID")
                    .HasColumnName("id");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Login date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");
                entity.Property(e => e.IpHash)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsFixedLength()
                    .HasComment("Connection IP address")
                    .HasColumnName("ip_hash");
                entity.Property(e => e.MinutesPlayed)
                    .HasDefaultValue(0)
                    .HasComment("Minutes played for session")
                    .HasColumnName("minutes_played");
                entity.Property(e => e.PenguinId)
                    .HasComment("Login penguin ID")
                    .HasColumnName("penguin_id");

                entity.HasOne(d => d.Penguin).WithMany(p => p.Logins)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("login_ibfk_1");
            });

            modelBuilder.Entity<Penguin>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("penguin_pkey");

                entity.ToTable("penguin", tb => tb.HasComment("Penguins"));

                entity.HasIndex(e => e.Email, "penguin_email");

                entity.HasIndex(e => e.Username, "penguin_username").IsUnique();

                entity.Property(e => e.Id)
                    .HasComment("Unique penguin ID")
                    .HasColumnName("id");
                entity.Property(e => e.Active)
                    .HasDefaultValue(false)
                    .HasComment("email activated")
                    .HasColumnName("active");
                entity.Property(e => e.AgentMedals)
                    .HasDefaultValue(0)
                    .HasComment("Current medals")
                    .HasColumnName("agent_medals");
                entity.Property(e => e.AgentStatus)
                    .HasDefaultValue(false)
                    .HasComment("Is penguin EPF agent?")
                    .HasColumnName("agent_status");
                entity.Property(e => e.ApprovalDe)
                    .HasDefaultValue(false)
                    .HasComment("Dutch username approval")
                    .HasColumnName("approval_de");
                entity.Property(e => e.ApprovalEn)
                    .HasDefaultValue(false)
                    .HasComment("English username approval")
                    .HasColumnName("approval_en");
                entity.Property(e => e.ApprovalEs)
                    .HasDefaultValue(false)
                    .HasComment("Spanish username approval")
                    .HasColumnName("approval_es");
                entity.Property(e => e.ApprovalFr)
                    .HasDefaultValue(false)
                    .HasComment("French username approval")
                    .HasColumnName("approval_fr");
                entity.Property(e => e.ApprovalPt)
                    .HasDefaultValue(false)
                    .HasComment("Portuguese username approval")
                    .HasColumnName("approval_pt");
                entity.Property(e => e.ApprovalRu)
                    .HasDefaultValue(false)
                    .HasComment("Russian username approval")
                    .HasColumnName("approval_ru");
                entity.Property(e => e.Body)
                    .HasComment("Penguin body item ID")
                    .HasColumnName("body");
                entity.Property(e => e.BookColor)
                    .HasDefaultValue(1)
                    .HasComment("Stampbook cover color")
                    .HasColumnName("book_color");
                entity.Property(e => e.BookHighlight)
                    .HasDefaultValue(1)
                    .HasComment("Stampbook highlight color")
                    .HasColumnName("book_highlight");
                entity.Property(e => e.BookIcon)
                    .HasDefaultValue(1)
                    .HasComment("Stampbook cover icon")
                    .HasColumnName("book_icon");
                entity.Property(e => e.BookModified)
                    .HasDefaultValue(0)
                    .HasComment("Is book cover modified?")
                    .HasColumnName("book_modified");
                entity.Property(e => e.BookPattern)
                    .HasDefaultValue(0)
                    .HasComment("Stampbook cover pattern")
                    .HasColumnName("book_pattern");
                entity.Property(e => e.CareerMedals)
                    .HasDefaultValue(0)
                    .HasComment("Total career medals")
                    .HasColumnName("career_medals");
                entity.Property(e => e.Character)
                    .HasComment("Character ID")
                    .HasColumnName("character");
                entity.Property(e => e.Coins)
                    .HasDefaultValue(500)
                    .HasComment("Penguin coins")
                    .HasColumnName("coins");
                entity.Property(e => e.Color)
                    .HasComment("Penguin color ID")
                    .HasColumnName("color");
                entity.Property(e => e.ComMessageReadDate)
                    .HasDefaultValueSql("now()")
                    .HasComment("Recent agent message read")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("com_message_read_date");
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("User Email address")
                    .HasColumnName("email");
                entity.Property(e => e.Face)
                    .HasComment("Penguin face item ID")
                    .HasColumnName("face");
                entity.Property(e => e.Feet)
                    .HasComment("Penguin feet item ID")
                    .HasColumnName("feet");
                entity.Property(e => e.FieldOpStatus)
                    .HasDefaultValue(0)
                    .HasComment("Is field op complete?")
                    .HasColumnName("field_op_status");
                entity.Property(e => e.FireMatchesWon)
                    .HasDefaultValue(0)
                    .HasComment("JitsuFire matches won")
                    .HasColumnName("fire_matches_won");
                entity.Property(e => e.FireNinjaProgress)
                    .HasDefaultValue(0)
                    .HasComment("Fire ninja progress")
                    .HasColumnName("fire_ninja_progress");
                entity.Property(e => e.FireNinjaRank)
                    .HasDefaultValue(0)
                    .HasComment("Fire ninja rank")
                    .HasColumnName("fire_ninja_rank");
                entity.Property(e => e.Flag)
                    .HasComment("Penguin pin ID")
                    .HasColumnName("flag");
                entity.Property(e => e.Grounded)
                    .HasDefaultValue(false)
                    .HasComment("Is player grounded?")
                    .HasColumnName("grounded");
                entity.Property(e => e.Hand)
                    .HasComment("Penguin hand item ID")
                    .HasColumnName("hand");
                entity.Property(e => e.HasDug)
                    .HasDefaultValue(false)
                    .HasComment("Puffle digging boolean")
                    .HasColumnName("has_dug");
                entity.Property(e => e.Head)
                    .HasComment("Penguin head item ID")
                    .HasColumnName("head");
                entity.Property(e => e.Igloo)
                    .HasComment("Penguin active igloo ID")
                    .HasColumnName("igloo");
                entity.Property(e => e.LastFieldOp)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of last field op")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("last_field_op");
                entity.Property(e => e.LastPaycheck)
                    .HasDefaultValueSql("now()")
                    .HasComment("EPF previous paycheck")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("last_paycheck");
                entity.Property(e => e.MapCategory)
                    .HasDefaultValue(0)
                    .HasComment("Currently selected map category")
                    .HasColumnName("map_category");
                entity.Property(e => e.MinutesPlayed)
                    .HasDefaultValue(0)
                    .HasComment("Total minutes connected")
                    .HasColumnName("minutes_played");
                entity.Property(e => e.Moderator)
                    .HasDefaultValue(false)
                    .HasComment("Is user moderator?")
                    .HasColumnName("moderator");
                entity.Property(e => e.Neck)
                    .HasComment("Penguin neck item ID")
                    .HasColumnName("neck");
                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Penguin display name")
                    .HasColumnName("nickname");
                entity.Property(e => e.NinjaMatchesWon)
                    .HasDefaultValue(0)
                    .HasComment("CardJitsu matches won")
                    .HasColumnName("ninja_matches_won");
                entity.Property(e => e.NinjaProgress)
                    .HasDefaultValue(0)
                    .HasComment("Ninja progress")
                    .HasColumnName("ninja_progress");
                entity.Property(e => e.NinjaRank)
                    .HasDefaultValue(0)
                    .HasComment("Ninja rank")
                    .HasColumnName("ninja_rank");
                entity.Property(e => e.Nuggets)
                    .HasDefaultValue(0)
                    .HasComment("Golden puffle nuggets")
                    .HasColumnName("nuggets");
                entity.Property(e => e.OpenedPlayercard)
                    .HasDefaultValue(false)
                    .HasComment("Has player opened playercard?")
                    .HasColumnName("opened_playercard");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsFixedLength()
                    .HasComment("Password hash")
                    .HasColumnName("password");
                entity.Property(e => e.Permaban)
                    .HasDefaultValue(false)
                    .HasComment("Is penguin banned forever?")
                    .HasColumnName("permaban");
                entity.Property(e => e.Photo)
                    .HasComment("Penguin background ID")
                    .HasColumnName("photo");
                entity.Property(e => e.PuffleHandler)
                    .HasDefaultValue(false)
                    .HasComment("Has met puffle handler?")
                    .HasColumnName("puffle_handler");
                entity.Property(e => e.RainbowAdoptability)
                    .HasDefaultValue(false)
                    .HasComment("Rainbow puffle adoptability status")
                    .HasColumnName("rainbow_adoptability");
                entity.Property(e => e.RegistrationDate)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of registration")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("registration_date");
                entity.Property(e => e.RejectionDe)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_de");
                entity.Property(e => e.RejectionEn)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_en");
                entity.Property(e => e.RejectionEs)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_es");
                entity.Property(e => e.RejectionFr)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_fr");
                entity.Property(e => e.RejectionPt)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_pt");
                entity.Property(e => e.RejectionRu)
                    .HasDefaultValue(false)
                    .HasColumnName("rejection_ru");
                entity.Property(e => e.SafeChat)
                    .HasDefaultValue(false)
                    .HasColumnName("safe_chat");
                entity.Property(e => e.SnowNinjaProgress)
                    .HasDefaultValue(0)
                    .HasColumnName("snow_ninja_progress");
                entity.Property(e => e.SnowNinjaRank)
                    .HasDefaultValue(0)
                    .HasColumnName("snow_ninja_rank");
                entity.Property(e => e.SnowProgressFireWins)
                    .HasDefaultValue(0)
                    .HasColumnName("snow_progress_fire_wins");
                entity.Property(e => e.SnowProgressSnowWins)
                    .HasDefaultValue(0)
                    .HasColumnName("snow_progress_snow_wins");
                entity.Property(e => e.SnowProgressWaterWins)
                    .HasDefaultValue(0)
                    .HasColumnName("snow_progress_water_wins");
                entity.Property(e => e.SpecialDance)
                    .HasDefaultValue(false)
                    .HasColumnName("special_dance");
                entity.Property(e => e.SpecialSnowball)
                    .HasDefaultValue(false)
                    .HasColumnName("special_snowball");
                entity.Property(e => e.SpecialWave)
                    .HasDefaultValue(false)
                    .HasColumnName("special_wave");
                entity.Property(e => e.StatusField)
                    .HasDefaultValue(0)
                    .HasComment("New player status field")
                    .HasColumnName("status_field");
                entity.Property(e => e.StealthModerator)
                    .HasDefaultValue(false)
                    .HasColumnName("stealth_moderator");
                entity.Property(e => e.TimerActive)
                    .HasDefaultValue(false)
                    .HasComment("Is egg-timer active?")
                    .HasColumnName("timer_active");
                entity.Property(e => e.TimerEnd)
                    .HasDefaultValueSql("'23:59:59'::time without time zone")
                    .HasComment("Egg-timer end time")
                    .HasColumnName("timer_end");
                entity.Property(e => e.TimerStart)
                    .HasComment("Egg-timer start time")
                    .HasColumnName("timer_start");
                entity.Property(e => e.TimerTotal)
                    .HasDefaultValueSql("'01:00:00'::interval")
                    .HasComment("Egg-timer total play time")
                    .HasColumnName("timer_total");
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasComment("Penguin login name")
                    .HasColumnName("username");
                entity.Property(e => e.Walking)
                    .HasComment("Walking puffle ID")
                    .HasColumnName("walking");
                entity.Property(e => e.WaterMatchesWon)
                    .HasDefaultValue(0)
                    .HasComment("JitsuWater matces won")
                    .HasColumnName("water_matches_won");
                entity.Property(e => e.WaterNinjaProgress)
                    .HasDefaultValue(0)
                    .HasComment("Water ninja progress")
                    .HasColumnName("water_ninja_progress");
                entity.Property(e => e.WaterNinjaRank)
                    .HasDefaultValue(0)
                    .HasComment("Water ninja rank")
                    .HasColumnName("water_ninja_rank");

                entity.HasOne(d => d.BodyNavigation).WithMany(p => p.PenguinBodyNavigations)
                    .HasForeignKey(d => d.Body)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_5");

                entity.HasOne(d => d.CharacterNavigation).WithMany(p => p.Penguins)
                    .HasForeignKey(d => d.Character)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_10");

                entity.HasOne(d => d.ColorNavigation).WithMany(p => p.PenguinColorNavigations)
                    .HasForeignKey(d => d.Color)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_1");

                entity.HasOne(d => d.FaceNavigation).WithMany(p => p.PenguinFaceNavigations)
                    .HasForeignKey(d => d.Face)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_3");

                entity.HasOne(d => d.FeetNavigation).WithMany(p => p.PenguinFeetNavigations)
                    .HasForeignKey(d => d.Feet)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_7");

                entity.HasOne(d => d.FlagNavigation).WithMany(p => p.PenguinFlagNavigations)
                    .HasForeignKey(d => d.Flag)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_9");

                entity.HasOne(d => d.HandNavigation).WithMany(p => p.PenguinHandNavigations)
                    .HasForeignKey(d => d.Hand)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_6");

                entity.HasOne(d => d.HeadNavigation).WithMany(p => p.PenguinHeadNavigations)
                    .HasForeignKey(d => d.Head)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_2");

                entity.HasOne(d => d.IglooNavigation).WithMany(p => p.Penguins)
                    .HasForeignKey(d => d.Igloo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_11");

                entity.HasOne(d => d.NeckNavigation).WithMany(p => p.PenguinNeckNavigations)
                    .HasForeignKey(d => d.Neck)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_4");

                entity.HasOne(d => d.PhotoNavigation).WithMany(p => p.PenguinPhotoNavigations)
                    .HasForeignKey(d => d.Photo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_8");

                entity.HasOne(d => d.WalkingNavigation).WithMany(p => p.Penguins)
                    .HasForeignKey(d => d.Walking)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_ibfk_12");

                entity.HasMany(d => d.Books).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinRedemptionBook",
                        r => r.HasOne<RedemptionBook>().WithMany()
                            .HasForeignKey("BookId")
                            .HasConstraintName("penguin_redemption_book_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_redemption_book_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "BookId").HasName("penguin_redemption_book_pkey");
                            j.ToTable("penguin_redemption_book", tb => tb.HasComment("Redeemed book codes"));
                            j.HasIndex(new[] { "BookId" }, "penguin_redemption_book_book_id");
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Unique penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("BookId")
                                .HasComment("Unique book ID")
                                .HasColumnName("book_id");
                        });

                entity.HasMany(d => d.Codes).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinRedemptionCode",
                        r => r.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("penguin_redemption_code_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_redemption_code_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "CodeId").HasName("penguin_redemption_code_pkey");
                            j.ToTable("penguin_redemption_code", tb => tb.HasComment("Redeemed codes"));
                            j.HasIndex(new[] { "CodeId" }, "penguin_redemption_code_code_id");
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Unique penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                        });

                entity.HasMany(d => d.Floorings).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinFlooring",
                        r => r.HasOne<Flooring>().WithMany()
                            .HasForeignKey("FlooringId")
                            .HasConstraintName("penugin_flooring_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_flooring_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "FlooringId").HasName("penguin_flooring_pkey");
                            j.ToTable("penguin_flooring", tb => tb.HasComment("Penguin owned furniture"));
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Owner penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("FlooringId")
                                .HasComment("Flooring item ID")
                                .HasColumnName("flooring_id");
                        });

                entity.HasMany(d => d.Igloos).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinIgloo",
                        r => r.HasOne<Igloo>().WithMany()
                            .HasForeignKey("IglooId")
                            .HasConstraintName("penguin_igloo_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_igloo_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "IglooId").HasName("penguin_igloo_pkey");
                            j.ToTable("penguin_igloo", tb => tb.HasComment("Penguin owned igloos"));
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Owner penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("IglooId")
                                .HasComment("Igloo ID")
                                .HasColumnName("igloo_id");
                        });

                entity.HasMany(d => d.Ignores).WithMany(p => p.PenguinsNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "IgnoreList",
                        r => r.HasOne<Penguin>().WithMany()
                            .HasForeignKey("IgnoreId")
                            .HasConstraintName("ignore_list_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("ignore_list_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "IgnoreId").HasName("ignore_list_pkey");
                            j.ToTable("ignore_list", tb => tb.HasComment("Penguin ignore relationships"));
                            j.HasIndex(new[] { "IgnoreId" }, "ignore_list_ignore_id");
                            j.IndexerProperty<int>("PenguinId").HasColumnName("penguin_id");
                            j.IndexerProperty<int>("IgnoreId").HasColumnName("ignore_id");
                        });

                entity.HasMany(d => d.Items).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinItem",
                        r => r.HasOne<Item>().WithMany()
                            .HasForeignKey("ItemId")
                            .HasConstraintName("penguin_item_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "ItemId").HasName("penguin_item_pkey");
                            j.ToTable("penguin_item", tb => tb.HasComment("Penguin owned clothing items"));
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Owner penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("ItemId")
                                .HasComment("Clothing item ID")
                                .HasColumnName("item_id");
                        });

                entity.HasMany(d => d.Locations).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinLocation",
                        r => r.HasOne<Location>().WithMany()
                            .HasForeignKey("LocationId")
                            .HasConstraintName("penguin_location_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_location_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "LocationId").HasName("penguin_location_pkey");
                            j.ToTable("penguin_location", tb => tb.HasComment("Penguin owned locations"));
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Owner penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<int>("LocationId")
                                .HasComment("Location ID")
                                .HasColumnName("location_id");
                        });

                entity.HasMany(d => d.Penguins).WithMany(p => p.Requesters)
                    .UsingEntity<Dictionary<string, object>>(
                        "BuddyRequest",
                        r => r.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("buddy_request_ibfk_1"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("RequesterId")
                            .HasConstraintName("buddy_request_ibfk_2"),
                        j =>
                        {
                            j.HasKey("PenguinId", "RequesterId").HasName("buddy_request_pkey");
                            j.ToTable("buddy_request", tb => tb.HasComment("Penguin buddy requests"));
                            j.IndexerProperty<int>("PenguinId").HasColumnName("penguin_id");
                            j.IndexerProperty<int>("RequesterId").HasColumnName("requester_id");
                        });

                entity.HasMany(d => d.PenguinsNavigation).WithMany(p => p.Ignores)
                    .UsingEntity<Dictionary<string, object>>(
                        "IgnoreList",
                        r => r.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("ignore_list_ibfk_1"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("IgnoreId")
                            .HasConstraintName("ignore_list_ibfk_2"),
                        j =>
                        {
                            j.HasKey("PenguinId", "IgnoreId").HasName("ignore_list_pkey");
                            j.ToTable("ignore_list", tb => tb.HasComment("Penguin ignore relationships"));
                            j.HasIndex(new[] { "IgnoreId" }, "ignore_list_ignore_id");
                            j.IndexerProperty<int>("PenguinId").HasColumnName("penguin_id");
                            j.IndexerProperty<int>("IgnoreId").HasColumnName("ignore_id");
                        });

                entity.HasMany(d => d.PermissionNames).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "PenguinPermission",
                        r => r.HasOne<Permission>().WithMany()
                            .HasForeignKey("PermissionName")
                            .HasConstraintName("penguin_permission_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("penguin_permission_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "PermissionName").HasName("penguin_permission_pkey");
                            j.ToTable("penguin_permission", tb => tb.HasComment("Penguin permissions"));
                            j.IndexerProperty<int>("PenguinId")
                                .HasComment("Penguin ID")
                                .HasColumnName("penguin_id");
                            j.IndexerProperty<string>("PermissionName")
                                .HasMaxLength(50)
                                .HasComment("Penguin permission name")
                                .HasColumnName("permission_name");
                        });

                entity.HasMany(d => d.Requesters).WithMany(p => p.Penguins)
                    .UsingEntity<Dictionary<string, object>>(
                        "BuddyRequest",
                        r => r.HasOne<Penguin>().WithMany()
                            .HasForeignKey("RequesterId")
                            .HasConstraintName("buddy_request_ibfk_2"),
                        l => l.HasOne<Penguin>().WithMany()
                            .HasForeignKey("PenguinId")
                            .HasConstraintName("buddy_request_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PenguinId", "RequesterId").HasName("buddy_request_pkey");
                            j.ToTable("buddy_request", tb => tb.HasComment("Penguin buddy requests"));
                            j.IndexerProperty<int>("PenguinId").HasColumnName("penguin_id");
                            j.IndexerProperty<int>("RequesterId").HasColumnName("requester_id");
                        });
            });

            modelBuilder.Entity<PenguinAttribute>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.PenguinId }).HasName("penguin_attribute_pkey");

                entity.ToTable("penguin_attribute", tb => tb.HasComment("Custom penguin attributes"));

                entity.Property(e => e.Name)
                    .HasComment("Attribute unique identifier")
                    .HasColumnName("name");
                entity.Property(e => e.PenguinId)
                    .HasComment("Penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasComment("Value of attribute")
                    .HasColumnName("value");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinAttributes)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_attribute_ibfk_1");
            });

            modelBuilder.Entity<PenguinCard>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.CardId }).HasName("penguin_card_pkey");

                entity.ToTable("penguin_card", tb => tb.HasComment("Penguin Card Jitsu decks"));

                entity.HasIndex(e => e.PenguinId, "penguin_card_penguin_id");

                entity.Property(e => e.PenguinId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.CardId)
                    .HasComment("Card type ID")
                    .HasColumnName("card_id");
                entity.Property(e => e.MemberQuantity)
                    .HasDefaultValue(0)
                    .HasComment("Quantity owned as member")
                    .HasColumnName("member_quantity");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasComment("Quantity owned")
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Card).WithMany(p => p.PenguinCards)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("penguin_card_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinCards)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_card_ibfk_1");
            });

            modelBuilder.Entity<PenguinFurniture>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.FurnitureId }).HasName("penguin_furniture_pkey");

                entity.ToTable("penguin_furniture", tb => tb.HasComment("Penguin owned furniture"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.FurnitureId)
                    .HasComment("Furniture item ID")
                    .HasColumnName("furniture_id");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasComment("Quantity owned")
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Furniture).WithMany(p => p.PenguinFurnitures)
                    .HasForeignKey(d => d.FurnitureId)
                    .HasConstraintName("penguin_furniture_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinFurnitures)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_furniture_ibfk_1");
            });

            modelBuilder.Entity<PenguinGameDatum>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.RoomId, e.Index }).HasName("penguin_game_data_pkey");

                entity.ToTable("penguin_game_data");

                entity.Property(e => e.PenguinId).HasColumnName("penguin_id");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.Index).HasColumnName("index");
                entity.Property(e => e.Data)
                    .HasDefaultValueSql("''::text")
                    .HasColumnName("data");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinGameData)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_game_data_ibfk_1");

                entity.HasOne(d => d.Room).WithMany(p => p.PenguinGameData)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("penguin_game_data_ibfk_2");
            });

            modelBuilder.Entity<PenguinIglooRoom>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("penguin_igloo_room_pkey");

                entity.ToTable("penguin_igloo_room", tb => tb.HasComment("Penguin igloo settings"));

                entity.Property(e => e.Id)
                    .HasComment("Unique igloo ID")
                    .HasColumnName("id");
                entity.Property(e => e.Competition)
                    .HasDefaultValue(false)
                    .HasComment("Is entered in competition?")
                    .HasColumnName("competition");
                entity.Property(e => e.Flooring)
                    .HasComment("Igloo flooring ID")
                    .HasColumnName("flooring");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.Locked)
                    .HasDefaultValue(true)
                    .HasComment("Is igloo locked?")
                    .HasColumnName("locked");
                entity.Property(e => e.Music)
                    .HasDefaultValue(0)
                    .HasComment("Igloo music ID")
                    .HasColumnName("music");
                entity.Property(e => e.PenguinId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Type)
                    .HasComment("Igloo type ID")
                    .HasColumnName("type");

                entity.HasOne(d => d.FlooringNavigation).WithMany(p => p.PenguinIglooRooms)
                    .HasForeignKey(d => d.Flooring)
                    .HasConstraintName("igloo_room_ibfk_3");

                entity.HasOne(d => d.LocationNavigation).WithMany(p => p.PenguinIglooRooms)
                    .HasForeignKey(d => d.Location)
                    .HasConstraintName("igloo_room_ibfk_4");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinIglooRooms)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("igloo_room_ibfk_1");

                entity.HasOne(d => d.TypeNavigation).WithMany(p => p.PenguinIglooRooms)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("igloo_room_ibfk_2");
            });

            modelBuilder.Entity<PenguinMembership>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.Start }).HasName("penguin_membership_pkey");

                entity.ToTable("penguin_membership", tb => tb.HasComment("Penguin membership records"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Penguin ID of membership")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Start)
                    .HasComment("Start time of membership")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start");
                entity.Property(e => e.ExpiredAware)
                    .HasDefaultValue(false)
                    .HasColumnName("expired_aware");
                entity.Property(e => e.Expires)
                    .HasComment("End time of membership")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("expires");
                entity.Property(e => e.ExpiresAware)
                    .HasDefaultValue(false)
                    .HasColumnName("expires_aware");
                entity.Property(e => e.StartAware)
                    .HasDefaultValue(false)
                    .HasColumnName("start_aware");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinMemberships)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_membership_ibfk_1");
            });

            modelBuilder.Entity<PenguinPostcard>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("penguin_postcard_pkey");

                entity.ToTable("penguin_postcard", tb => tb.HasComment("Sent postcards"));

                entity.HasIndex(e => e.PenguinId, "penguin_postcard_penguin_id");

                entity.HasIndex(e => e.SenderId, "penguin_postcard_sender_id");

                entity.Property(e => e.Id)
                    .HasComment("Unique postcard ID")
                    .HasColumnName("id");
                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Postcard details")
                    .HasColumnName("details");
                entity.Property(e => e.HasRead)
                    .HasDefaultValue(false)
                    .HasComment("Is read?")
                    .HasColumnName("has_read");
                entity.Property(e => e.PenguinId)
                    .HasComment("Postcard type ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.PostcardId)
                    .HasComment("Postcard type ID")
                    .HasColumnName("postcard_id");
                entity.Property(e => e.SendDate)
                    .HasDefaultValueSql("now()")
                    .HasComment("Postcard type ID")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("send_date");
                entity.Property(e => e.SenderId)
                    .HasComment("Sender penguin ID")
                    .HasColumnName("sender_id");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinPostcardPenguins)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_postcard_ibfk_1");

                entity.HasOne(d => d.Postcard).WithMany(p => p.PenguinPostcards)
                    .HasForeignKey(d => d.PostcardId)
                    .HasConstraintName("penguin_postcard_ibfk_3");

                entity.HasOne(d => d.Sender).WithMany(p => p.PenguinPostcardSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_postcard_ibfk_2");
            });

            modelBuilder.Entity<PenguinPuffle>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("penguin_puffle_pkey");

                entity.ToTable("penguin_puffle", tb => tb.HasComment("Adopted puffles"));

                entity.Property(e => e.Id)
                    .HasComment("Unique puffle ID")
                    .HasColumnName("id");
                entity.Property(e => e.AdoptionDate)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of adoption")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("adoption_date");
                entity.Property(e => e.Backyard)
                    .HasDefaultValue(false)
                    .HasComment("Is in backyard?")
                    .HasColumnName("backyard");
                entity.Property(e => e.Clean)
                    .HasDefaultValue(100)
                    .HasComment("Puffle clean %")
                    .HasColumnName("clean");
                entity.Property(e => e.Food)
                    .HasDefaultValue(100)
                    .HasComment("Puffle health %")
                    .HasColumnName("food");
                entity.Property(e => e.HasDug)
                    .HasDefaultValue(false)
                    .HasComment("Has dug?")
                    .HasColumnName("has_dug");
                entity.Property(e => e.Hat)
                    .HasComment("Puffle hat item ID")
                    .HasColumnName("hat");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasComment("Puffle name")
                    .HasColumnName("name");
                entity.Property(e => e.PenguinId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Play)
                    .HasDefaultValue(100)
                    .HasComment("Puffle hunger %")
                    .HasColumnName("play");
                entity.Property(e => e.PuffleId)
                    .HasComment("Puffle type ID")
                    .HasColumnName("puffle_id");
                entity.Property(e => e.Rest)
                    .HasDefaultValue(100)
                    .HasComment("Puffle rest %")
                    .HasColumnName("rest");

                entity.HasOne(d => d.HatNavigation).WithMany(p => p.PenguinPuffles)
                    .HasForeignKey(d => d.Hat)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("penguin_puffle_ibfk_3");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinPuffles)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_puffle_ibfk_1");

                entity.HasOne(d => d.Puffle).WithMany(p => p.PenguinPuffles)
                    .HasForeignKey(d => d.PuffleId)
                    .HasConstraintName("penguin_puffle_ibfk_2");
            });

            modelBuilder.Entity<PenguinPuffleItem>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.ItemId }).HasName("penguin_puffle_item_pkey");

                entity.ToTable("penguin_puffle_item", tb => tb.HasComment("Owned puffle care items"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.ItemId)
                    .HasComment("Puffle care item ID")
                    .HasColumnName("item_id");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasComment("Quantity owned")
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Item).WithMany(p => p.PenguinPuffleItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("penguin_puffle_item_ibfk_2");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinPuffleItems)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_puffle_item_ibfk_1");
            });

            modelBuilder.Entity<PenguinQuestTask>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.PenguinId }).HasName("penguin_quest_task_pkey");

                entity.ToTable("penguin_quest_task", tb => tb.HasComment("Completed quest tasks"));

                entity.Property(e => e.TaskId)
                    .HasComment("Completed task ID")
                    .HasColumnName("task_id");
                entity.Property(e => e.PenguinId)
                    .HasComment("Task penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Complete)
                    .HasDefaultValue(false)
                    .HasColumnName("complete");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinQuestTasks)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("penguin_quest_task_ibfk_2");

                entity.HasOne(d => d.Task).WithMany(p => p.PenguinQuestTasks)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("penguin_quest_task_ibfk_1");
            });

            modelBuilder.Entity<PenguinStamp>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.StampId }).HasName("penguin_stamp_pkey");

                entity.ToTable("penguin_stamp", tb => tb.HasComment("Penguin earned stamps"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Stamp penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.StampId)
                    .HasComment("Stamp ID")
                    .HasColumnName("stamp_id");
                entity.Property(e => e.Recent)
                    .HasDefaultValue(true)
                    .HasComment("Is recently earned?")
                    .HasColumnName("recent");

                entity.HasOne(d => d.Penguin).WithMany(p => p.PenguinStamps)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("stamp_ibfk_1");

                entity.HasOne(d => d.Stamp).WithMany(p => p.PenguinStamps)
                    .HasForeignKey(d => d.StampId)
                    .HasConstraintName("stamp_ibfk_2");
            });

            modelBuilder.Entity<PenguinTrack>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("penguin_track_pkey");

                entity.ToTable("penguin_track", tb => tb.HasComment("Penguin SoundStudio tracks"));

                entity.Property(e => e.Id)
                    .HasComment("Unique track ID")
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Name of track")
                    .HasColumnName("name");
                entity.Property(e => e.OwnerId)
                    .HasComment("Owner penguin ID")
                    .HasColumnName("owner_id");
                entity.Property(e => e.Pattern)
                    .IsRequired()
                    .HasComment("Song data")
                    .HasColumnName("pattern");
                entity.Property(e => e.Sharing)
                    .HasDefaultValue(false)
                    .HasComment("Is song shared?")
                    .HasColumnName("sharing");

                entity.HasOne(d => d.Owner).WithMany(p => p.PenguinTracks)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("penguin_track_ibfk_1");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Name).HasName("permission_pkey");

                entity.ToTable("permission", tb => tb.HasComment("Registered server permissions"));

                entity.HasIndex(e => e.Name, "permission_name").IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Unique permission identifier")
                    .HasColumnName("name");
                entity.Property(e => e.Enabled)
                    .HasDefaultValue(true)
                    .HasColumnName("enabled");
            });

            modelBuilder.Entity<PluginAttribute>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.PluginName }).HasName("plugin_attribute_pkey");

                entity.ToTable("plugin_attribute", tb => tb.HasComment("Custom plugin attributes"));

                entity.Property(e => e.Name)
                    .HasComment("Attribute unique identifier")
                    .HasColumnName("name");
                entity.Property(e => e.PluginName)
                    .HasComment("Name of plugin attribute belongs to")
                    .HasColumnName("plugin_name");
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasComment("Value of attribute")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Postcard>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("postcard_pkey");

                entity.ToTable("postcard", tb => tb.HasComment("Server postcard crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique postcard ID")
                    .HasColumnName("id");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(10)
                    .HasComment("Cost of postcard")
                    .HasColumnName("cost");
                entity.Property(e => e.Enabled)
                    .HasDefaultValue(false)
                    .HasComment("Can send postcard?")
                    .HasColumnName("enabled");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Postcard name")
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Puffle>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("puffle_pkey");

                entity.ToTable("puffle", tb => tb.HasComment("Server puffle crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique puffle ID")
                    .HasColumnName("id");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Puffle cost")
                    .HasColumnName("cost");
                entity.Property(e => e.FavouriteFood)
                    .HasComment("Favourite puffle-care item")
                    .HasColumnName("favourite_food");
                entity.Property(e => e.FavouriteToy).HasColumnName("favourite_toy");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Puffle name")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("Base color puffle ID")
                    .HasColumnName("parent_id");
                entity.Property(e => e.RunawayPostcard)
                    .HasComment("Runaway postcard ID")
                    .HasColumnName("runaway_postcard");

                entity.HasOne(d => d.FavouriteFoodNavigation).WithMany(p => p.PuffleFavouriteFoodNavigations)
                    .HasForeignKey(d => d.FavouriteFood)
                    .HasConstraintName("puffle_ibfk_2");

                entity.HasOne(d => d.FavouriteToyNavigation).WithMany(p => p.PuffleFavouriteToyNavigations)
                    .HasForeignKey(d => d.FavouriteToy)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("puffle_ibfk_3");

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("puffle_ibfk_1");

                entity.HasOne(d => d.RunawayPostcardNavigation).WithMany(p => p.Puffles)
                    .HasForeignKey(d => d.RunawayPostcard)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("puffle_ibfk_4");

                entity.HasMany(d => d.Furnitures).WithMany(p => p.Puffles)
                    .UsingEntity<Dictionary<string, object>>(
                        "PuffleTreasureFurniture",
                        r => r.HasOne<Furniture>().WithMany()
                            .HasForeignKey("FurnitureId")
                            .HasConstraintName("puffle_treasure_furniture_ibfk_2"),
                        l => l.HasOne<Puffle>().WithMany()
                            .HasForeignKey("PuffleId")
                            .HasConstraintName("puffle_treasure_furniture_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PuffleId", "FurnitureId").HasName("puffle_treasure_furniture_pkey");
                            j.ToTable("puffle_treasure_furniture", tb => tb.HasComment("Puffle digging treasure furniture"));
                            j.IndexerProperty<int>("PuffleId")
                                .HasComment("Puffle type ID")
                                .HasColumnName("puffle_id");
                            j.IndexerProperty<int>("FurnitureId")
                                .HasComment("Furniture item ID")
                                .HasColumnName("furniture_id");
                        });

                entity.HasMany(d => d.Items).WithMany(p => p.Puffles)
                    .UsingEntity<Dictionary<string, object>>(
                        "PuffleTreasureItem",
                        r => r.HasOne<Item>().WithMany()
                            .HasForeignKey("ItemId")
                            .HasConstraintName("puffle_treasure_item_ibfk_2"),
                        l => l.HasOne<Puffle>().WithMany()
                            .HasForeignKey("PuffleId")
                            .HasConstraintName("puffle_treasure_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PuffleId", "ItemId").HasName("puffle_treasure_item_pkey");
                            j.ToTable("puffle_treasure_item", tb => tb.HasComment("Puffle digging treasure clothing"));
                            j.IndexerProperty<int>("PuffleId")
                                .HasComment("Puffle type ID")
                                .HasColumnName("puffle_id");
                            j.IndexerProperty<int>("ItemId")
                                .HasComment("Clothing item ID")
                                .HasColumnName("item_id");
                        });

                entity.HasMany(d => d.PuffleItems).WithMany(p => p.Puffles)
                    .UsingEntity<Dictionary<string, object>>(
                        "PuffleTreasurePuffleItem",
                        r => r.HasOne<PuffleItem>().WithMany()
                            .HasForeignKey("PuffleItemId")
                            .HasConstraintName("puffle_treasure_puffle_item_ibfk_2"),
                        l => l.HasOne<Puffle>().WithMany()
                            .HasForeignKey("PuffleId")
                            .HasConstraintName("puffle_treasure_puffle_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("PuffleId", "PuffleItemId").HasName("puffle_treasure_puffle_item_pkey");
                            j.ToTable("puffle_treasure_puffle_item", tb => tb.HasComment("Puffle digging treasure puffle care items"));
                            j.IndexerProperty<int>("PuffleId")
                                .HasComment("Puffle type ID")
                                .HasColumnName("puffle_id");
                            j.IndexerProperty<int>("PuffleItemId")
                                .HasComment("Puffle care item ID")
                                .HasColumnName("puffle_item_id");
                        });
            });

            modelBuilder.Entity<PuffleItem>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("puffle_item_pkey");

                entity.ToTable("puffle_item", tb => tb.HasComment("Server puffle care item crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique care item ID")
                    .HasColumnName("id");
                entity.Property(e => e.CleanEffect)
                    .HasDefaultValue(0)
                    .HasComment("Effect on puffle clean level")
                    .HasColumnName("clean_effect");
                entity.Property(e => e.Cost)
                    .HasDefaultValue(0)
                    .HasComment("Cost of care item")
                    .HasColumnName("cost");
                entity.Property(e => e.FoodEffect)
                    .HasDefaultValue(0)
                    .HasComment("Effect on puffle food level")
                    .HasColumnName("food_effect");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Care item name")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("Parent care item ID")
                    .HasColumnName("parent_id");
                entity.Property(e => e.PlayEffect)
                    .HasDefaultValue(0)
                    .HasComment("Effect on puffle play level")
                    .HasColumnName("play_effect");
                entity.Property(e => e.PlayExternal)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'none'::character varying")
                    .HasComment("External play mode")
                    .HasColumnName("play_external");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasComment("Base quantity of purchase")
                    .HasColumnName("quantity");
                entity.Property(e => e.RestEffect)
                    .HasDefaultValue(0)
                    .HasComment("Effect on puffle rest level")
                    .HasColumnName("rest_effect");
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'care'::character varying")
                    .HasComment("Type of care item")
                    .HasColumnName("type");

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("puffle_item_ibfk_1");
            });

            modelBuilder.Entity<Quest>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("quest_pkey");

                entity.ToTable("quest", tb => tb.HasComment("Player map quests"));

                entity.Property(e => e.Id)
                    .HasComment("Unique quest ID")
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasComment("Short name of quest")
                    .HasColumnName("name");

                entity.HasMany(d => d.Items).WithMany(p => p.Quests)
                    .UsingEntity<Dictionary<string, object>>(
                        "QuestAwardItem",
                        r => r.HasOne<Item>().WithMany()
                            .HasForeignKey("ItemId")
                            .HasConstraintName("quest_award_item_ibfk_2"),
                        l => l.HasOne<Quest>().WithMany()
                            .HasForeignKey("QuestId")
                            .HasConstraintName("quest_award_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("QuestId", "ItemId").HasName("quest_award_item_pkey");
                            j.ToTable("quest_award_item");
                            j.IndexerProperty<int>("QuestId")
                                .HasComment("Quest ID")
                                .HasColumnName("quest_id");
                            j.IndexerProperty<int>("ItemId")
                                .HasComment("Clothing item ID")
                                .HasColumnName("item_id");
                        });
            });

            modelBuilder.Entity<QuestAwardFurniture>(entity =>
            {
                entity.HasKey(e => new { e.QuestId, e.FurnitureId }).HasName("quest_award_furniture_pkey");

                entity.ToTable("quest_award_furniture");

                entity.Property(e => e.QuestId)
                    .HasComment("Quest ID")
                    .HasColumnName("quest_id");
                entity.Property(e => e.FurnitureId)
                    .HasComment("Furniture item ID")
                    .HasColumnName("furniture_id");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasColumnName("quantity");

                entity.HasOne(d => d.Furniture).WithMany(p => p.QuestAwardFurnitures)
                    .HasForeignKey(d => d.FurnitureId)
                    .HasConstraintName("quest_award_furniture_ibfk_2");

                entity.HasOne(d => d.Quest).WithMany(p => p.QuestAwardFurnitures)
                    .HasForeignKey(d => d.QuestId)
                    .HasConstraintName("quest_award_furniture_ibfk_1");
            });

            modelBuilder.Entity<QuestAwardPuffleItem>(entity =>
            {
                entity.HasKey(e => new { e.QuestId, e.PuffleItemId }).HasName("quest_award_puffle_item_pkey");

                entity.ToTable("quest_award_puffle_item");

                entity.Property(e => e.QuestId)
                    .HasComment("Quest ID")
                    .HasColumnName("quest_id");
                entity.Property(e => e.PuffleItemId)
                    .HasComment("Puffle care item ID")
                    .HasColumnName("puffle_item_id");
                entity.Property(e => e.Quantity)
                    .HasDefaultValue(1)
                    .HasColumnName("quantity");

                entity.HasOne(d => d.PuffleItem).WithMany(p => p.QuestAwardPuffleItems)
                    .HasForeignKey(d => d.PuffleItemId)
                    .HasConstraintName("quest_award_puffle_item_ibfk_2");

                entity.HasOne(d => d.Quest).WithMany(p => p.QuestAwardPuffleItems)
                    .HasForeignKey(d => d.QuestId)
                    .HasConstraintName("quest_award_puffle_item_ibfk_1");
            });

            modelBuilder.Entity<QuestTask>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("quest_task_pkey");

                entity.ToTable("quest_task", tb => tb.HasComment("Player map quest tasks"));

                entity.Property(e => e.Id)
                    .HasComment("Unique task ID")
                    .HasColumnName("id");
                entity.Property(e => e.Data)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("NULL::character varying")
                    .HasColumnName("data");
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Description of task")
                    .HasColumnName("description");
                entity.Property(e => e.QuestId)
                    .HasComment("Task quest ID")
                    .HasColumnName("quest_id");
                entity.Property(e => e.RoomId)
                    .HasComment("Room ID for completion")
                    .HasColumnName("room_id");

                entity.HasOne(d => d.Quest).WithMany(p => p.QuestTasks)
                    .HasForeignKey(d => d.QuestId)
                    .HasConstraintName("quest_task_ibfk_1");

                entity.HasOne(d => d.Room).WithMany(p => p.QuestTasks)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("quest_task_ibfk_2");
            });

            modelBuilder.Entity<RedemptionBook>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("redemption_book_pkey");

                entity.ToTable("redemption_book", tb => tb.HasComment("Redemption books"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique redemption book ID")
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("Book name")
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RedemptionBookWord>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.Page, e.Line, e.WordNumber }).HasName("redemption_book_word_pkey");

                entity.ToTable("redemption_book_word", tb => tb.HasComment("Redemption book answers"));

                entity.Property(e => e.BookId)
                    .HasComment("Redemption book ID")
                    .HasColumnName("book_id");
                entity.Property(e => e.Page)
                    .HasDefaultValue(1)
                    .HasComment("Page number inside book")
                    .HasColumnName("page");
                entity.Property(e => e.Line)
                    .HasDefaultValue(1)
                    .HasComment("Line number of page")
                    .HasColumnName("line");
                entity.Property(e => e.WordNumber)
                    .HasDefaultValue(1)
                    .HasComment("The nth word on the line")
                    .HasColumnName("word_number");
                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("The correct word")
                    .HasColumnName("answer");
                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.HasOne(d => d.Book).WithMany(p => p.RedemptionBookWords)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("redemption_book_word_ibfk_1");
            });

            modelBuilder.Entity<RedemptionCode>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("redemption_code_pkey");

                entity.ToTable("redemption_code", tb => tb.HasComment("Redemption codes"));

                entity.HasIndex(e => e.Code, "redemption_code_code").IsUnique();

                entity.Property(e => e.Id)
                    .HasComment("Unique code ID")
                    .HasColumnName("id");
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasComment("Redemption code")
                    .HasColumnName("code");
                entity.Property(e => e.Coins)
                    .HasDefaultValue(0)
                    .HasComment("Code coins amount")
                    .HasColumnName("coins");
                entity.Property(e => e.Expires)
                    .HasComment("Expiry date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("expires");
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasDefaultValueSql("'BLANKET'::character varying")
                    .HasComment("Code type")
                    .HasColumnName("type");
                entity.Property(e => e.Uses)
                    .HasComment("Number of uses")
                    .HasColumnName("uses");

                entity.HasMany(d => d.Cards).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardCard",
                        r => r.HasOne<Card>().WithMany()
                            .HasForeignKey("CardId")
                            .HasConstraintName("redemption_award_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "CardId").HasName("redemption_award_card_pkey");
                            j.ToTable("redemption_award_card", tb => tb.HasComment("Redemption code card awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("CardId")
                                .HasComment("Code card ID")
                                .HasColumnName("card_id");
                        });

                entity.HasMany(d => d.Floorings).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardFlooring",
                        r => r.HasOne<Flooring>().WithMany()
                            .HasForeignKey("FlooringId")
                            .HasConstraintName("redemption_award_flooring_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_flooring_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "FlooringId").HasName("redemption_award_flooring_pkey");
                            j.ToTable("redemption_award_flooring", tb => tb.HasComment("Redemption code flooring awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("FlooringId")
                                .HasComment("Code igloo flooring ID")
                                .HasColumnName("flooring_id");
                        });

                entity.HasMany(d => d.Furnitures).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardFurniture",
                        r => r.HasOne<Furniture>().WithMany()
                            .HasForeignKey("FurnitureId")
                            .HasConstraintName("redemption_award_furniture_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_furniture_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "FurnitureId").HasName("redemption_award_furniture_pkey");
                            j.ToTable("redemption_award_furniture", tb => tb.HasComment("Redemption code furniture awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("FurnitureId")
                                .HasComment("Code igloo furniture ID")
                                .HasColumnName("furniture_id");
                        });

                entity.HasMany(d => d.Igloos).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardIgloo",
                        r => r.HasOne<Igloo>().WithMany()
                            .HasForeignKey("IglooId")
                            .HasConstraintName("redemption_award_igloo_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_igloo_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "IglooId").HasName("redemption_award_igloo_pkey");
                            j.ToTable("redemption_award_igloo", tb => tb.HasComment("Redemption code igloo awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("IglooId")
                                .HasComment("Code igloo ID")
                                .HasColumnName("igloo_id");
                        });

                entity.HasMany(d => d.Items).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardItem",
                        r => r.HasOne<Item>().WithMany()
                            .HasForeignKey("ItemId")
                            .HasConstraintName("redemption_award_item_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "ItemId").HasName("redemption_award_item_pkey");
                            j.ToTable("redemption_award_item", tb => tb.HasComment("Redemption code item awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("ItemId")
                                .HasComment("Code item ID")
                                .HasColumnName("item_id");
                        });

                entity.HasMany(d => d.Locations).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardLocation",
                        r => r.HasOne<Location>().WithMany()
                            .HasForeignKey("LocationId")
                            .HasConstraintName("redemption_award_location_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_location_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "LocationId").HasName("redemption_award_location_pkey");
                            j.ToTable("redemption_award_location", tb => tb.HasComment("Redemption code location awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("LocationId")
                                .HasComment("Code igloo location ID")
                                .HasColumnName("location_id");
                        });

                entity.HasMany(d => d.PuffleItems).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardPuffleItem",
                        r => r.HasOne<PuffleItem>().WithMany()
                            .HasForeignKey("PuffleItemId")
                            .HasConstraintName("redemption_award_puffle_item_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_puffle_item_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "PuffleItemId").HasName("redemption_award_puffle_item_pkey");
                            j.ToTable("redemption_award_puffle_item", tb => tb.HasComment("Redemption code puffle care item awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("PuffleItemId")
                                .HasComment("Code puffle care item ID")
                                .HasColumnName("puffle_item_id");
                        });

                entity.HasMany(d => d.Puffles).WithMany(p => p.Codes)
                    .UsingEntity<Dictionary<string, object>>(
                        "RedemptionAwardPuffle",
                        r => r.HasOne<Puffle>().WithMany()
                            .HasForeignKey("PuffleId")
                            .HasConstraintName("redemption_award_puffle_ibfk_2"),
                        l => l.HasOne<RedemptionCode>().WithMany()
                            .HasForeignKey("CodeId")
                            .HasConstraintName("redemption_award_puffle_ibfk_1"),
                        j =>
                        {
                            j.HasKey("CodeId", "PuffleId").HasName("redemption_award_puffle_pkey");
                            j.ToTable("redemption_award_puffle", tb => tb.HasComment("Redemption code puffle awards"));
                            j.IndexerProperty<int>("CodeId")
                                .HasComment("Unique code ID")
                                .HasColumnName("code_id");
                            j.IndexerProperty<int>("PuffleId")
                                .HasComment("Code puffle ID")
                                .HasColumnName("puffle_id");
                        });
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("report_pkey");

                entity.ToTable("report", tb => tb.HasComment("Player reports"));

                entity.Property(e => e.Id)
                    .HasComment("Unique report ID")
                    .HasColumnName("id");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Date of report")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");
                entity.Property(e => e.PenguinId)
                    .HasComment("Reported penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.ReportType)
                    .HasDefaultValue(0)
                    .HasComment("Report type ID")
                    .HasColumnName("report_type");
                entity.Property(e => e.ReporterId)
                    .HasComment("Reporting penguin ID")
                    .HasColumnName("reporter_id");
                entity.Property(e => e.RoomId).HasColumnName("room_id");
                entity.Property(e => e.ServerId).HasColumnName("server_id");

                entity.HasOne(d => d.Penguin).WithMany(p => p.ReportPenguins)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("report_ibfk_1");

                entity.HasOne(d => d.Reporter).WithMany(p => p.ReportReporters)
                    .HasForeignKey(d => d.ReporterId)
                    .HasConstraintName("report_ibfk_2");

                entity.HasOne(d => d.Room).WithMany(p => p.Reports)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("report_ibfk_3");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("room_pkey");

                entity.ToTable("room", tb => tb.HasComment("Server room crumbs"));

                entity.HasIndex(e => e.InternalId, "room_internal_id").IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique room ID")
                    .HasColumnName("id");
                entity.Property(e => e.Blackhole)
                    .HasDefaultValue(false)
                    .HasComment("Is blackhole game room?")
                    .HasColumnName("blackhole");
                entity.Property(e => e.Game)
                    .HasDefaultValue(false)
                    .HasComment("Is game room?")
                    .HasColumnName("game");
                entity.Property(e => e.InternalId)
                    .ValueGeneratedOnAdd()
                    .HasComment("Internal room key")
                    .HasColumnName("internal_id");
                entity.Property(e => e.MaxUsers)
                    .HasDefaultValue(80)
                    .HasComment("Maximum room users")
                    .HasColumnName("max_users");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Room name")
                    .HasColumnName("name");
                entity.Property(e => e.RequiredItem)
                    .HasComment("Required inventory item")
                    .HasColumnName("required_item");
                entity.Property(e => e.Spawn)
                    .HasDefaultValue(false)
                    .HasComment("Is spawn room?")
                    .HasColumnName("spawn");
                entity.Property(e => e.StampGroup).HasColumnName("stamp_group");

                entity.HasOne(d => d.RequiredItemNavigation).WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.RequiredItem)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("room_ibfk_1");

                entity.HasOne(d => d.StampGroupNavigation).WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.StampGroup)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("room_ibfk_2");
            });

            modelBuilder.Entity<RoomTable>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.RoomId }).HasName("room_table_pkey");

                entity.ToTable("room_table", tb => tb.HasComment("Server table games"));

                entity.Property(e => e.Id)
                    .HasComment("Table ID")
                    .HasColumnName("id");
                entity.Property(e => e.RoomId)
                    .HasComment("Room ID of table")
                    .HasColumnName("room_id");
                entity.Property(e => e.Game)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Game of table")
                    .HasColumnName("game");

                entity.HasOne(d => d.Room).WithMany(p => p.RoomTables)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("room_table_ibfk_1");
            });

            modelBuilder.Entity<RoomWaddle>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.RoomId }).HasName("room_waddle_pkey");

                entity.ToTable("room_waddle", tb => tb.HasComment("Server waddle games"));

                entity.Property(e => e.Id)
                    .HasComment("Waddle ID")
                    .HasColumnName("id");
                entity.Property(e => e.RoomId)
                    .HasComment("Room ID of waddle")
                    .HasColumnName("room_id");
                entity.Property(e => e.Game)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Game of waddle")
                    .HasColumnName("game");
                entity.Property(e => e.Seats)
                    .HasDefaultValue(2)
                    .HasComment("Number of seats")
                    .HasColumnName("seats");

                entity.HasOne(d => d.Room).WithMany(p => p.RoomWaddles)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("room_waddle_ibfk_1");
            });

            modelBuilder.Entity<Stamp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("stamp_pkey");

                entity.ToTable("stamp", tb => tb.HasComment("Server stamp crumbs"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique stamp ID")
                    .HasColumnName("id");
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("''::character varying")
                    .HasComment("Stamp description")
                    .HasColumnName("description");
                entity.Property(e => e.GroupId)
                    .HasComment("Stamp group ID")
                    .HasColumnName("group_id");
                entity.Property(e => e.Member)
                    .HasDefaultValue(false)
                    .HasComment("Is member-only?")
                    .HasColumnName("member");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Stamp name")
                    .HasColumnName("name");
                entity.Property(e => e.Rank)
                    .HasDefaultValue(1)
                    .HasComment("Stamp difficulty ranking")
                    .HasColumnName("rank");

                entity.HasOne(d => d.Group).WithMany(p => p.Stamps)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("stamp_ibfk_1");
            });

            modelBuilder.Entity<StampGroup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("stamp_group_pkey");

                entity.ToTable("stamp_group", tb => tb.HasComment("Stamp group collections"));

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("Unique stamp group ID")
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Name of stamp group")
                    .HasColumnName("name");
                entity.Property(e => e.ParentId)
                    .HasComment("Parent stamp group ID")
                    .HasColumnName("parent_id");

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("stamp_group_ibfk_1");
            });

            modelBuilder.Entity<TrackLike>(entity =>
            {
                entity.HasKey(e => new { e.TrackId, e.PenguinId, e.Date }).HasName("track_like_pkey");

                entity.ToTable("track_like", tb => tb.HasComment("SoundStudio likes"));

                entity.HasIndex(e => e.TrackId, "track_like_track_id");

                entity.Property(e => e.TrackId)
                    .HasComment("Liked track ID")
                    .HasColumnName("track_id");
                entity.Property(e => e.PenguinId)
                    .HasComment("Liker penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("now()")
                    .HasComment("Timestamp of like")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.HasOne(d => d.Penguin).WithMany(p => p.TrackLikes)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("track_like_ibfk_1");

                entity.HasOne(d => d.Track).WithMany(p => p.TrackLikes)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("track_like_ibfk_2");
            });

            modelBuilder.Entity<Warning>(entity =>
            {
                entity.HasKey(e => new { e.PenguinId, e.Issued, e.Expires }).HasName("warning_pkey");

                entity.ToTable("warning", tb => tb.HasComment("Penguin moderator warnings"));

                entity.Property(e => e.PenguinId)
                    .HasComment("Warning penguin ID")
                    .HasColumnName("penguin_id");
                entity.Property(e => e.Issued)
                    .HasComment("Warning issue date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("issued");
                entity.Property(e => e.Expires)
                    .HasComment("Warning expiry date")
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("expires");

                entity.HasOne(d => d.Penguin).WithMany(p => p.Warnings)
                    .HasForeignKey(d => d.PenguinId)
                    .HasConstraintName("warning_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}