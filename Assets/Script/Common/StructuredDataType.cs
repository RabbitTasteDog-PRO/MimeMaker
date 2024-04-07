using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace StructuredDataType
{

    ///<summary>
    /// 스케줄 체크용 구조체
    ///</summary>
    public class WeeklyScheduleData
    {
        public eWeekly weekly = eWeekly.NONE;
        public eAct act = eAct.NONE;
        public eSchedule schedule = eSchedule.NONE;

    }

}

