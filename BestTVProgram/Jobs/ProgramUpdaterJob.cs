using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using BestTVProgram.Services;

namespace BestTVProgram.Jobs
{
    class ProgramUpdaterJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => TVProgramService.RefreshChannelsWithProgram());
            
        }
    }
}
