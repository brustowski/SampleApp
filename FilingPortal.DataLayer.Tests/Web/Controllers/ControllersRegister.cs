using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Controllers.Admin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Web.Controllers
{
    [TestClass]
    public class ControllersRegister
    {
        [TestInitialize]
        public void Init()
        {
            _controllerPermissionsList = new List<ControllerPermissions>
            {
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.Pipeline"),
                    Permissions = new List<Permissions>()
                    {
                        Permissions.PipelineImportInboundRecord,
                        Permissions.PipelineViewInboundRecord,
                        Permissions.PipelineDeleteInboundRecord,
                        Permissions.PipelineFileInboundRecord,
                        Permissions.PipelineViewInboundRecordRules,
                        Permissions.PipelineEditInboundRecordRules,
                        Permissions.PipelineDeleteInboundRecordRules,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                    }
                }, // Pipeline
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.Rail"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.RailViewInboundRecord,
                        Permissions.RailDeleteInboundRecord,
                        Permissions.RailViewManifest,
                        Permissions.RailFileInboundRecord,
                        Permissions.RailViewInboundRecordRules,
                        Permissions.RailEditInboundRecordRules,
                        Permissions.RailDeleteInboundRecordRules,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                    }
                }, // Rail
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.Truck"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.TruckViewInboundRecord,
                        Permissions.TruckDeleteInboundRecord,
                        Permissions.TruckFileInboundRecord,
                        Permissions.TruckViewInboundRecordRules,
                        Permissions.TruckEditInboundRecordRules,
                        Permissions.TruckDeleteInboundRecordRules,
                        Permissions.TruckImportInboundRecord,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                    }
                }, // Truck
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.TruckExport"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.TruckViewExportRecord,
                        Permissions.TruckDeleteExportRecord,
                        Permissions.TruckFileExportRecord,
                        Permissions.TruckViewExportRecordRules,
                        Permissions.TruckEditExportRecordRules,
                        Permissions.TruckDeleteExportRecordRules,
                        Permissions.TruckImportExportRecord,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                        Permissions.AdminAutoCreateConfiguration
                    }
                }, // Truck Export
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.Vessel"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.VesselViewImportRecord,
                        Permissions.VesselDeleteImportRecord,
                        Permissions.VesselFileImportRecord,
                        Permissions.VesselViewImportRecordRules,
                        Permissions.VesselEditImportRecordRules,
                        Permissions.VesselDeleteImportRecordRules,
                        Permissions.VesselFileImportRecord,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                        Permissions.VesselAddImportRecord
                    }
                }, // Vessel
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.VesselExport"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.VesselViewExportRecord,
                        Permissions.VesselDeleteExportRecord,
                        Permissions.VesselFileExportRecord,
                        Permissions.VesselViewExportRecordRules,
                        Permissions.VesselEditExportRecordRules,
                        Permissions.VesselDeleteExportRecordRules,
                        Permissions.ViewConfiguration,
                        Permissions.EditConfiguration,
                        Permissions.DeleteConfiguration,
                        Permissions.VesselAddExportRecord
                    }
                }, // Vessel Export
                new ControllerPermissions
                {
                    Controllers = new List<Type>{typeof(ClientsController) },
                    Permissions = new List<Permissions>
                    {
                        Permissions.ViewClients
                    }
                }, // ClientsController
                new ControllerPermissions
                {
                    Controllers = GetTypesInNamespace("FilingPortal.Web.Controllers.Audit.Rail"),
                    Permissions = new List<Permissions>
                    {
                        Permissions.AuditRailDailyAudit,
                        Permissions.AuditRailImportTrainConsistSheet
                    }, // Rail audit
                },
                new ControllerPermissions
                {
                    Controllers = new List<Type>{typeof(AutoFilingGridController) },
                    Permissions = new List<Permissions>
                {
                    Permissions.AdminAutoCreateConfiguration
                }
                }, // Admins controller
                };
        }

        private List<ControllerPermissions> _controllerPermissionsList;

        public bool IsRegistered<T>() where T : ApiController
        {
            return _controllerPermissionsList.Any(x => x.Controllers.Contains(typeof(T)));
        }
        private static List<Type> GetTypesInNamespace(string nameSpace)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.ManifestModule.Name == "FilingPortal.Web.dll");
            return
                assembly.GetTypes()
                    .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                    .ToList();
        }

        public List<Permissions> GetAvailablePermissions(Type type)
        {
            IEnumerable<ControllerPermissions> permissions = _controllerPermissionsList.Where(x => x.Controllers.Contains(type));
            return permissions.SelectMany(x => x.Permissions).Distinct().ToList();
        }

        [TestMethod]
        public void All_Routed_Methods_Have_Their_Own_permissions()
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.ManifestModule.Name == "FilingPortal.Web.dll");
            var controllerTypes =
                assembly.GetTypes().Where(x => x.IsAssignableTo<ApiController>())
                    .ToList();

            foreach (Type controllerType in controllerTypes)
            {
                MethodInfo[] methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo methodInfo in methods)
                {
                    IEnumerable<Attribute> routeAttributes = methodInfo.GetCustomAttributes(typeof(RouteAttribute));
                    if (routeAttributes.Any())
                    {
                        var permissionAttributes = (IEnumerable<PermissionRequiredAttribute>)methodInfo.GetCustomAttributes(typeof(PermissionRequiredAttribute));
                        var allPermissions = permissionAttributes.Where(x => x.RequiredPermissions != null)
                                .SelectMany(x => x.RequiredPermissions).ToList();
                        List<Permissions> availablePermissions = GetAvailablePermissions(controllerType);
                        CollectionAssert.IsSubsetOf(allPermissions, availablePermissions,
                            $"Controller: {controllerType.FullName}, Method: {methodInfo.Name}, Expected: {string.Join(", ", availablePermissions.Cast<Permissions>())}, Actual: {string.Join(",", allPermissions.Cast<Permissions>())}");
                    }
                }
            }
        }


        internal class ControllerPermissions
        {
            public List<Type> Controllers { get; set; }
            public List<Permissions> Permissions { get; set; }
        }
    }
}
