using System.Web;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace TestPortalSiteMapNonPublishingSite
{
    public class Navigation : PortalSiteMapProvider
    {
       
        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            SiteMapNodeCollection siteMapNodes = new SiteMapNodeCollection();
            // cast from .net node to portalnode
            PortalSiteMapNode portalNode = node as PortalSiteMapNode;
            // check it
            if (portalNode == null)
                return siteMapNodes;
            TaxonomySession taxonomySession = new TaxonomySession(SPContext.Current.Site);
            TermStore termStore = taxonomySession.TermStores[0];
            Group termGroup = termStore.Groups["BeckyTermGroup"];
            TermSet termSet = termGroup.TermSets["BeckyTermSet"];

            // root
            if (node.Key.ToLower() == SPContext.Current.Web.ServerRelativeUrl.ToLower())
            {
                foreach (var term in termSet.Terms)
                {

                    siteMapNodes.Add(SetNavNode(portalNode, term));
                }
            }
            else
            {
                var subTerm = termSet.GetTerm(new Guid(node.Key));
                foreach (var term in subTerm.Terms)
                {

                    siteMapNodes.Add(SetNavNode(portalNode, term));
                }
            }

            return siteMapNodes;
        }

        private SiteMapNode SetNavNode(PortalSiteMapNode portalNode, Term termNode)
        {
            try
            {
                var friendlyURL = termNode.LocalCustomProperties["_Sys_Nav_SimpleLinkUrl"];
                var navNode = new PortalSiteMapNode(portalNode.WebNode,
             termNode.Id.ToString(),
                            NodeTypes.Heading,
                        friendlyURL,
                            termNode.Name,
                        string.Empty);
                return navNode;
            }
            catch (KeyNotFoundException ex)
            {
                var friendlyURL = string.Empty;
                var navNode = new PortalSiteMapNode(portalNode.WebNode,
             termNode.Id.ToString(),
                            NodeTypes.Heading,
                        friendlyURL,
                            termNode.Name,
                        string.Empty);
                return navNode;
            }


        }
    }
}

