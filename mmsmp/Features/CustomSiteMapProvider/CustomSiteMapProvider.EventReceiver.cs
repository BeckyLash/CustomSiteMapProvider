using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Collections.ObjectModel;

namespace TestPortalSiteMapNonPublishingSite.Features.CustomSiteMapProvider
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("01e782e9-47ba-4e12-ad16-9754773eb4d0")]
    public class CustomSiteMapProviderEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {

                SPWebService service = SPWebService.ContentService;
                SPWebConfigModification myModification = new SPWebConfigModification();
                myModification.Path = "configuration/system.web/siteMap/providers";
                myModification.Name = "add[@name='TestPortalSiteMapNonPublishingSite']";
                myModification.Sequence = 0;
                myModification.Owner = "TopNavCustomSiteMapProvider";
                myModification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                var typeName = typeof(TestPortalSiteMapNonPublishingSite.Navigation).FullName + ", " + typeof(TestPortalSiteMapNonPublishingSite.Navigation).Assembly.FullName;
                myModification.Value = "<add name=\"TestPortalSiteMapNonPublishingSite\" type=\"" + typeName + "\" NavigationType=\"Global\" />";
                service.WebConfigModifications.Add(myModification);
                service.Update();
                service.ApplyWebConfigModifications();
            });
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                SPWebService service = SPWebService.ContentService;
                Collection<SPWebConfigModification> modsCollection = service.WebConfigModifications;
                int modsCount1 = modsCollection.Count;
                for (int i = modsCount1 - 1; i > -1; i--)
                {
                    if (modsCollection[i].Owner.Equals("TopNavCustomSiteMapProvider"))
                    {
                        modsCollection.Remove(modsCollection[i]);
                    }
                }
                service.Update();
                service.ApplyWebConfigModifications();
            });
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
