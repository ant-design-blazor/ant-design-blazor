using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// 可以以两种方式倒计时,即传入两种数据类型之一做为参数： 
    /// 1.以现在时刻为起点的倒计时时间长度，类型为TimSpan 
    /// 2.倒计时截止到未来某时刻,类型为DateTime
    /// </summary>
    public partial class CountDown : StatisticComponentBase<OneOf<DateTime, TimeSpan>>
    {
        [Parameter] public string Format { get; set; } = @"hh\:mm\:ss";
        [Parameter] public EventCallback CountdownFinish { get; set; }

        private int Target { get; set; } = 0;

        private Timer _timer;
        /// <summary>
        /// 每1000毫秒刷新近似30次
        /// </summary>
        private const int REFRESH_INTERVAL = 1000 / 30;
        private TimeSpan _span = TimeSpan.Zero;

        protected override void OnInitialized()
        {
            if (Value.IsT0)
                Value = Value.AsT0 - DateTime.Now;

            _timer = new Timer(StartCountDownForTimeSpan);
            _timer.Change(0, REFRESH_INTERVAL);
        }
        void StartCountDownForTimeSpan(object o)
        {
            Value = Value.AsT1.Add(TimeSpan.FromMilliseconds(-REFRESH_INTERVAL));
            InvokeStateHasChanged();
            if (Value.AsT1.Ticks <= 0)
            {
                //因为是近似，此时Ticks已经可能为负值，所以要强制设置为0，否则Value值可能不是0
                Value = TimeSpan.Zero;
                InvokeStateHasChanged();
                _timer.Dispose();
                CountdownFinish.InvokeAsync(o);
            }
        }
    }
}


//ts 特殊符号用法

//1. 属性或参数中使用 ？：表示该属性或参数为可选项

//2. 属性或参数中使用 ！：表示强制解析（告诉typescript编译器，这里一定有值），常用于vue-decorator中的 @Prop

//3. 变量后使用 ！：表示类型推断排除null、undefined
