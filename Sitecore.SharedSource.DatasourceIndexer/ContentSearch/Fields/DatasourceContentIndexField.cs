﻿using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.SharedSource.DatasourceIndexer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Assert = Sitecore.Diagnostics.Assert;
using SC = Sitecore;

namespace Sitecore.SharedSource.DatasourceIndexer.ContentSearch.Fields
{
    public class DatasourceContentIndexField : Sitecore.ContentSearch.ComputedFields.IComputedIndexField
    {
        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            try
            {
                Assert.ArgumentNotNull(indexable, "indexable");
                SC.Data.Items.Item item = indexable as SC.ContentSearch.SitecoreIndexableItem;

                if (item == null || item.Database == null)
                {
                    //Log.Log.Warn(this + " : unsupported SitecoreIndexableItem type : " + scIndexable.GetType());
                    return null;
                }

                if (!item.Paths.IsContentItem)
                {
                    return null;
                }

                // optimization to reduce indexing time
                // by skipping this logic for items in the Core database
                if (System.String.Compare(item.Database.Name, "core", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return null;
                }

                string devicesString = Sitecore.Configuration.Settings.GetSetting(DSIUtil.SettingDevices, DSIUtil.DefaultDevice);
                if (string.IsNullOrWhiteSpace(devicesString))
                {
                    devicesString = DSIUtil.DefaultDevice;
                }

                Sitecore.Data.ID[] devices = devicesString.Split('|').Where(c => Sitecore.Data.ID.IsID(c)).Select(c => new Sitecore.Data.ID(new Guid(c))).ToArray();
                if (devices.Any())
                {
                    List<String> allFieldValues = new List<string>();

                    DeviceItem[] availableDevices = item.Database.Resources.Devices.GetAll();

                    foreach (Sitecore.Data.ID deviceID in devices)
                    {
                        var deviceItem = availableDevices.FirstOrDefault(d => d.ID == deviceID);
                        if (deviceItem != null)
                        {
                            var renderings = item.Visualization.GetRenderings(deviceItem, false);
                            renderings = renderings
                                .Where(r => r.Settings.DataSource != string.Empty)
                                .Where(r => r.RenderingItem != null).ToArray();

                            foreach (RenderingReference rr in renderings)
                            {
                                string fieldValue = rr.RenderingItem.InnerItem[DSIUtil.ControllerParamFields];
                                if (!string.IsNullOrWhiteSpace(fieldValue))
                                {
                                    var resolvedDatasourceItem = DSIUtil.ResolveDatasource(rr.Settings.DataSource, item);
                                    if (resolvedDatasourceItem != null)
                                    {
                                        allFieldValues.AddRange(fieldValue.Split('|').Select(fieldName => resolvedDatasourceItem[fieldName]));
                                    }
                                }
                            }
                        }
                    }

                    string returnValue = String.Join("|", allFieldValues.Where(c => !string.IsNullOrWhiteSpace(c)));

                    if (string.IsNullOrWhiteSpace(returnValue))
                        return null;

                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("Exception thrown at indexing: {0}", ex), this);
            }

            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}