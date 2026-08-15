//
// Copyright (c) 2010-2021 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System.Threading;

using AntShell.Terminal;

namespace Antmicro.Renode.UI
{
    // The monitor's input when there is no monitor: `--hide-monitor`, which
    // `--disable-gui` implies and which every `renode-test` instance runs with.
    //
    // A passive source is *pulled*: the shell's terminal loop calls `Read` and
    // blocks in it until a byte arrives. This one used to answer 0 immediately
    // and forever, which is not "no input" but "an endless stream of NUL
    // keystrokes" — so the shell thread spun at a full host core for the whole
    // life of the process, feeding NULs through the line editor. See
    // DUMMY-IO-SOURCE-SPIN-FIX.md.
    //
    // No input will ever arrive here, so `Read` waits instead, and returns
    // end-of-stream when a caller cancels it — the contract `StreamIOSource`
    // implements and the one `PAIOSourceConverter`/`NavigableTerminalEmulator`
    // expect.
    public class DummyIOSource : IPassiveIOSource
    {
        public void CancelRead()
        {
            cancelled.Set();
        }

        public void Dispose()
        {
            cancelled.Set();
        }

        public void Flush()
        {
        }

        // Nothing is ever waiting: `IOProvider.HasNewInput` asks this in passive
        // mode, and answering `true` claimed a byte was ready that `Read` then
        // invented.
        public bool TryPeek(out int value)
        {
            value = -1;
            return false;
        }

        public int Read()
        {
            cancelled.WaitOne();
            return -1;
        }

        public void Write(byte b)
        {
        }

        private readonly ManualResetEvent cancelled = new ManualResetEvent(false);
    }
}
