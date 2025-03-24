using System;
using System.Text;
using PenguinCS.Data;

namespace PenguinCS.Common.Extensions;

public static class PenguinExtensions
{
    public static int GetApproval(this Penguin player)
    {
        StringBuilder sb = new();
        sb.Append('0');
        sb.Append(player.ApprovalRu ? '1' : '0');
        sb.Append(player.ApprovalDe ? '1' : '0');
        sb.Append('0');
        sb.Append(player.ApprovalEs ? '1' : '0');
        sb.Append(player.ApprovalFr ? '1' : '0');
        sb.Append(player.ApprovalPt ? '1' : '0');
        sb.Append(player.ApprovalEn ? '1' : '0');

        return Convert.ToInt32(sb.ToString(), 2);
    }

    public static int GetRejection(this Penguin player)
    {
        StringBuilder sb = new();
        sb.Append('0');
        sb.Append(player.RejectionRu ? '1' : '0');
        sb.Append(player.RejectionDe ? '1' : '0');
        sb.Append('0');
        sb.Append(player.RejectionEs ? '1' : '0');
        sb.Append(player.RejectionFr ? '1' : '0');
        sb.Append(player.RejectionPt ? '1' : '0');
        sb.Append(player.RejectionEn ? '1' : '0');

        return Convert.ToInt32(sb.ToString(), 2);
    }

    public static int GetAgeInDays(this Penguin player)
    {
        return (DateTime.UtcNow - player.RegistrationDate).Days;
    }
}