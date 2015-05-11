﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncTrayzor.SyncThing.ApiClient
{
    public class DownloadProgressEventFileData
    {
        /// <summary>
        /// Total number of blocks in this file
        /// </summary>
        [JsonProperty("Total")]
        public long Total { get; set; }

        /// <summary>
        /// Number of blocks currently being downloaded
        /// </summary>
        [JsonProperty("Pulling")]
        public int Pulling { get; set; }

        /// <summary>
        /// Number of blocks copied from the file we are about to replace
        /// </summary>
        [JsonProperty("CopiedFromOrigin")]
        public int CopiedFromOrigin { get; set; }

        /// <summary>
        /// Number of blocks reused from a previous temporary file
        /// </summary>
        [JsonProperty("Reused")]
        public int Reused { get; set; }

        /// <summary>
        /// Number of blocks copied from other files or potentially other folders
        /// </summary>
        [JsonProperty("CopedFromElsewhere")]
        public int CopiedFromElsewhere { get; set; }

        /// <summary>
        /// Number of blocks actually downloaded so far
        /// </summary>
        [JsonProperty("Pulled")]
        public int Pulled { get; set; }

        /// <summary>
        /// Approximate total file size
        /// </summary>
        [JsonProperty("BytesTotal")]
        public long BytesTotal { get; set; }

        /// <summary>
        /// Approximate number of bytes already handled (already reused, copied, or pulled)
        /// </summary>
        [JsonProperty("BytesDone")]
        public long BytesDone { get; set; }
    }

    public class DownloadProgressEventFolderData
    {
        [JsonExtensionData]
        public Dictionary<string, DownloadProgressEventFileData> Files { get; set; }
    }

    public class DownloadProgressEventData
    {
        [JsonExtensionData]
        public Dictionary<string, DownloadProgressEventFolderData> Folders { get; set; }
    }

    public class DownloadProgressEvent : Event
    {
        [JsonProperty("data")]
        public DownloadProgressEventData Data { get; set; }

        public override void Visit(IEventVisitor visitor)
        {
            visitor.Accept(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var folder in this.Data.Folders)
            {
                foreach (var file in folder.Value.Files)
                {
                    sb.AppendFormat("{0}:{1}={2}/{3}", folder.Key, file.Key, file.Value.BytesDone, file.Value.BytesTotal);
                }
            }

            return String.Format("<DownloadProgress ID={0} Time={1} {2}>", this.Id, this.Time, sb.ToString());
        }
    }
}