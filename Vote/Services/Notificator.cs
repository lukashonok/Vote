using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.TargetModelService;

namespace Vote.Services
{
    public static class Notificator
    {
        static public void EvidenceEdited(INotificationModelService notificationModelService, CompromisingEvidenceModel evidence)
        {
            notificationModelService.InsertNotificationModel(new NotificationModel
            {
                ApplicationGetterId = evidence.UserId,
                CreatedAt = DateTime.Now,
                Message = $"EvidenceChanged"
            });
        }
        static public void UserEdited(INotificationModelService notificationModelService, ApplicationUser user)
        {
            notificationModelService.InsertNotificationModel(new NotificationModel
            {
                ApplicationGetterId = user,
                CreatedAt = DateTime.Now,
                Message = $"UserEdited"
            });
        }
        static public void VoteProcessChanged(INotificationModelService notificationModelService, UserManager<ApplicationUser> userManager)
        {
            IList<ApplicationUser> users = userManager.Users.ToList();
            foreach (var user in users)
            {
                notificationModelService.InsertNotificationModel(new NotificationModel
                {
                    ApplicationGetterId = user,
                    CreatedAt = DateTime.Now,
                    Message = $"VoteProcessChanged"
                });
            }
        }
    }
}
