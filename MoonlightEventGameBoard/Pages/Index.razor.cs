using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace MoonlightEventGameBoard.Pages
{
    using System.Threading;

    public partial class Index : IDisposable
    {
        // We can also capture the browser's width / height if needed. We hold the value here.
        private BrowserWindowSize browser = new();

        public Index()
        {
            this.Game = new Game();
        }

        [Inject]
        public ResizeListener Listener { get; private set; } = null!;

        [Inject]
        public IJSRuntime JsRuntime { get; private set; } = null!;

        public Game Game { get; }

        public int Size { get; set; }

        public double? DevicePixelRatio { get; set; }

        private Timer? updateTimer;

        void IDisposable.Dispose()
        {
            // Always use IDisposable in your component to unsubscribe from the event.
            // Be a good citizen and leave things how you found them. 
            // This way event handlers aren't called when nobody is listening.
            this.Listener.OnResized -= this.WindowResized;

            this.updateTimer?.Dispose();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                this.DevicePixelRatio = await this.JsRuntime.InvokeAsync<double>("getWindowDevicePixelRatio");

                // Subscribe to the OnResized event. This will do work when the browser is resized.
                this.Listener.OnResized += this.WindowResized;
                this.updateTimer = new Timer(_ => { this.StateHasChanged(); }, null, 1000, 1000);
            }
        }

        // This method will be called when the window resizes.
        // It is ONLY called when the user stops dragging the window's edge. (It is already throttled to protect your app from perf. nightmares)
        private void WindowResized(object? _, BrowserWindowSize window)
        {
            // Get the browser's width / height
            this.browser = window;
            this.CalcSize();
        }

        private void CalcSize()
        {
            var maxPartWidth = (int)((this.browser.Width / 4 * 0.8) / (2 * 1.5));
            //var maxPartHeight = 100000;
            var maxPartHeight = (int)((this.browser.Height * 0.9 * 0.2 / 2));

            this.Size = maxPartWidth < maxPartHeight ? maxPartWidth : maxPartHeight;
            this.StateHasChanged();
        }
    }
}
