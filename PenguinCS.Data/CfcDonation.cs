using System;
using System.Collections.Generic;

namespace PenguinCS.Data;

/// <summary>
/// CFC charity donations
/// </summary>
public partial class CfcDonation
{
    /// <summary>
    /// Donator penguin ID
    /// </summary>
    public int PenguinId { get; set; }

    /// <summary>
    /// Donation coin amount
    /// </summary>
    public int Coins { get; set; }

    /// <summary>
    /// Donation charity or cause
    /// </summary>
    public int Charity { get; set; }

    /// <summary>
    /// Date of donation
    /// </summary>
    public DateTime Date { get; set; }

    public virtual Penguin Penguin { get; set; }
}
