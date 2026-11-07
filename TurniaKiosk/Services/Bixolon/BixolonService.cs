using BIXOLON_SamplePg;
using Services.Queue.Dtos;
using System.Diagnostics;

namespace Services.Bixolon
{
    public class BixolonService
    {
        private bool _printReady;
        private int _printerStatus;
        private Timer _timer = null!;

        private readonly object _printLock = new();

        public bool PrinterDisabled { get; set; }

        public BixolonService()
        {
            PrinterInit();
        }


        #region Printer init Functions
        private void PrinterInit()
        {
            try
            {
                Debug.WriteLine($"PRINTER: Initializing");
                var result = BXLAPI.ConnectUsb();

                _timer = new Timer(callback: ShowPrinterStatus, null, 0, 5000);

                if (result == 0)
                {
                    _printReady = true;
                    Debug.WriteLine($"PRINTER: Connected/Ready");
                }
                else
                {
                    Debug.WriteLine($"PRINTER: Not Connected: Error code {result}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PRINTER: Not Connected: Error code {ex.Message}");
            }
        }

        #endregion


#region Printer Status Functions
        private void ShowPrinterStatus(object? state)
        {
            var printerStatus = BXLAPI.GetPrinterCurrentStatus();
            _printerStatus = printerStatus;
        }

        public bool IsPrinterReady()
        {
            return _printReady;
        }

        public int PrinterStatus()
        {
            return _printerStatus;
        }
        #endregion


        #region Print Ticket Functions
        public (bool, Exception?) PrintTicket(Ticket ticket)
        {
            if (PrinterDisabled)
            {
                Console.WriteLine($"PRINTER: Ticket {ticket.TicketNumber} ({ticket.Id}) print skipped (printer disabled for testing)");
                return (true, null);
            }

            if (_printReady)
            {
                try
                {
                    lock (_printLock)
                    {
                        BXLAPI.LineFeed(1);
                        BXLAPI.PrintText($"{ticket.TicketNumber}", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, 34);
                        BXLAPI.LineFeed(1);
                        BXLAPI.LineFeed(1);
                        BXLAPI.PrintText("Gracias", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, 144);
                        BXLAPI.LineFeed(1);
                        BXLAPI.PrintText("En breve le atenderemos", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH);
                        BXLAPI.LineFeed(1);
                        BXLAPI.LineFeed(1);
                        BXLAPI.PrintText(ticket.TicketDate.ToString("dd/MM/yyyy HH:mm"), BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH);
                        BXLAPI.LineFeed(1);
                        BXLAPI.PrintText("Turnia", BXLAPI.BXL_ALIGNMENT_CENTER, BXLAPI.BXL_FT_DEFAULT, BXLAPI.BXL_TS_0WIDTH);
                        BXLAPI.LineFeed(1);
                        BXLAPI.LineFeed(1);
                        BXLAPI.CutPaper();
                    }

                    Console.WriteLine($"PRINTER: Ticket {ticket.TicketNumber} ({ticket.Id}) was printed");
                    return (true, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PRINTER: Ticket {{ticket.TicketNumber}} ({{ticket.Id}}) was NOT printed. Error code {ex.Message}");
                    return (false, ex);
                }

            }
            return (false, null);
        }
        #endregion

    }
}
