using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Silmoon.Threading
{
    public class Debouncer
    {
        private readonly int _delayMilliseconds;
        private CancellationTokenSource _cts;

        public Debouncer(int delayMilliseconds = 300)
        {
            _delayMilliseconds = delayMilliseconds;
        }

        /// <summary>
        /// 执行一个需要防抖的异步操作。
        /// </summary>
        /// <param name="action">要执行的操作。</param>
        public void Debounce(Action action)
        {
            // 取消之前的延迟任务
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Task.Delay(_delayMilliseconds, token).ContinueWith(t =>
            {
                if (!t.IsCanceled) action?.Invoke();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 执行一个需要防抖的异步操作（异步版）。
        /// </summary>
        /// <param name="action">要执行的异步操作。</param>
        /// <returns></returns>
        public async void DebounceAsync(Func<Task> action)
        {
            // 取消之前的延迟任务
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                await Task.Delay(_delayMilliseconds, token);
                if (!token.IsCancellationRequested)
                {
                    await action?.Invoke();
                }
            }
            catch (TaskCanceledException)
            {
                // 忽略被取消的任务
            }
        }
    }
}
