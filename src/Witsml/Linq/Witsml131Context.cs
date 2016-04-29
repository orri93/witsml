﻿//----------------------------------------------------------------------- 
// PDS.Witsml, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Security;
using Energistics.DataAccess;
using Energistics.DataAccess.WITSML131;
using Energistics.Datatypes;

namespace PDS.Witsml.Linq
{
    public class Witsml131Context : WitsmlContext
    {
        public Witsml131Context(string url, double timeoutInMinutes = 1.5)
            : base(url, timeoutInMinutes, WMLSVersion.WITSML131)
        {
        }

        public Witsml131Context(string url, string username, string password, double timeoutInMinutes = 1.5)
            : base(url, username, password, timeoutInMinutes, WMLSVersion.WITSML131)
        {
        }

        public Witsml131Context(string url, string username, SecureString password, double timeoutInMinutes = 1.5)
            : base(url, username, password, timeoutInMinutes, WMLSVersion.WITSML131)
        {
        }

        public override string DataSchemaVersion
        {
            get { return OptionsIn.DataVersion.Version131.Value; }
        }

        public IWitsmlQuery<Well> Wells
        {
            get { return CreateQuery<Well, WellList>(); }
        }

        public IWitsmlQuery<Wellbore> Wellbores
        {
            get { return CreateQuery<Wellbore, WellboreList>(); }
        }

        public IWitsmlQuery<Rig> Rigs
        {
            get { return CreateQuery<Rig, RigList>(); }
        }

        public IWitsmlQuery<Log> Logs
        {
            get { return CreateQuery<Log, LogList>(); }
        }

        public IWitsmlQuery<Trajectory> Trajectories
        {
            get { return CreateQuery<Trajectory, TrajectoryList>(); }
        }

        public override IEnumerable<IDataObject> GetAllWells()
        {
            return Wells.With(OptionsIn.ReturnElements.IdOnly)
                .ToList() // execute query before sorting
                .OrderBy(x => x.Name);
        }

        public override IEnumerable<IWellObject> GetWellbores(string uri)
        {
            var etpUri = new EtpUri(uri);

            return Wellbores.With(OptionsIn.ReturnElements.IdOnly)
                .Where(x => x.UidWell == etpUri.ObjectId)
                .ToList() // execute query before sorting
                .OrderBy(x => x.Name);
        }
    }
}
