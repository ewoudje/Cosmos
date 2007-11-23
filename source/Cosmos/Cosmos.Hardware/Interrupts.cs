﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cosmos.Hardware {
	public class Interrupts: Hardware {
		[StructLayout(LayoutKind.Sequential)]
		public struct InterruptContext {
			public uint SS;
			public uint GS;
			public uint FS;
			public uint ES;
			public uint DS;
			public uint EDI;
			public uint ESI;
			public uint EBP;
			public uint ESP;
			public uint EBX;
			public uint EDX;
			public uint ECX;
			public uint EAX;
			public byte Interrupt;
			public uint Param;
			public uint EIP;
			public uint CS;
			public uint EFlags;
			public uint UserESP;
		}

		public unsafe static void HandleInterrupt_Default(InterruptContext* aContext) {
			Console.Write("Interrupt ");
			WriteNumber(aContext->Interrupt, 32);
			Console.WriteLine("");
			Serial.DebugWrite("Interrupt ");
			WriteNumber(aContext->Interrupt, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    SS = ");
			WriteNumber(aContext->SS, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    GS = ");
			WriteNumber(aContext->GS, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    FS = ");
			WriteNumber(aContext->FS, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    ES = ");
			WriteNumber(aContext->ES, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    DS = ");
			WriteNumber(aContext->DS, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EDI = ");
			WriteNumber(aContext->EDI, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    ESI = ");
			WriteNumber(aContext->ESI, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EBP = ");
			WriteNumber(aContext->EBP, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    ESP = ");
			WriteNumber(aContext->ESP, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EBX = ");
			WriteNumber(aContext->EBX, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EDX = ");
			WriteNumber(aContext->EDX, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    ECX = ");
			WriteNumber(aContext->ECX, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EAX = ");
			WriteNumber(aContext->EAX, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    Param = ");
			WriteNumber(aContext->Param, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EIP = ");
			WriteNumber(aContext->EIP, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    CS = ");
			WriteNumber(aContext->CS, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    EFlags = ");
			WriteNumber(aContext->EFlags, 32);
			Serial.DebugWriteLine("");
			Serial.DebugWrite("    UserESP = ");
			WriteNumber(aContext->UserESP, 32);
			Serial.DebugWriteLine("");
			if (aContext->Interrupt >= 0x20 && aContext->Interrupt <= 0x2F) {
				if (aContext->Interrupt >= 0x28) {
					PIC.SignalSecondary();
				} else {
					PIC.SignalPrimary();
				}
			}
		}

		//IRQ 2 - Cascaded signals from IRQs 8-15. A device configured to use IRQ 2 will actually be using IRQ 9
		//IRQ 3 - COM2 (Default) and COM4 (User) serial ports
		//IRQ 4 - COM1 (Default) and COM3 (User) serial ports
		//IRQ 5 - LPT2 Parallel Port 2 or sound card
		//IRQ 6 - Floppy disk controller
		//IRQ 7 - LPT1 Parallel Port 1 or sound card (8-bit Sound Blaster and compatibles)

		//IRQ 8 - Real time clock
		//IRQ 9 - Free / Open interrupt / Available / SCSI. Any devices configured to use IRQ 2 will actually be using IRQ 9.
		//IRQ 10 - Free / Open interrupt / Available / SCSI
		//IRQ 11 - Free / Open interrupt / Available / SCSI
		//IRQ 12 - PS/2 connector Mouse. If no PS/2 connector mouse is used, this can be used for other peripherals
		//IRQ 13 - ISA / Math Co-Processor
		//IRQ 14 - Primary IDE. If no Primary IDE this can be changed
		//IRQ 15 - Secondary IDE

		//IRQ 0 - System timer. Reserved for the system. Cannot be changed by a user.
		public static unsafe void HandleInterrupt_20(InterruptContext* aContext) {
			Console.WriteLine("PIT IRQ occurred");
			PIC.SignalPrimary();
		}

		//IRQ 1 - Keyboard. Reserved for the system. Cannot be altered even if no keyboard is present or needed.
		public static unsafe void HandleInterrupt_21(InterruptContext* aContext) {
			byte xScanCode = IORead(0x60);
			Serial.DebugWrite("Keyboard Interrupt, ScanCode = ");
			WriteNumber(xScanCode, 8);
			Serial.DebugWriteLine("");
			PIC.SignalPrimary();
		}

		// This is to trick IL2CPU to compile it in
		//TODO: Make a new attribute that IL2CPU sees when scanning to force inclusion so we dont have to do this.
		public static void IncludeAllHandlers() {
			bool xTest = false;
			if (xTest) {
				unsafe {
					HandleInterrupt_Default(null);
					HandleInterrupt_20(null);
					HandleInterrupt_21(null);
				}
			}
		}

		private static void WriteNumber(uint aValue, byte aBitCount) {
			uint xValue = aValue;
			byte xCurrentBits = aBitCount;
			Serial.DebugWrite("0x");
			while (xCurrentBits >= 4) {
				xCurrentBits -= 4;
				byte xCurrentDigit = (byte)((xValue >> xCurrentBits) & 0xF);
				string xDigitString = null;
				switch (xCurrentDigit) {
					case 0:
						xDigitString = "0";
						goto default;
					case 1:
						xDigitString = "1";
						goto default;
					case 2:
						xDigitString = "2";
						goto default;
					case 3:
						xDigitString = "3";
						goto default;
					case 4:
						xDigitString = "4";
						goto default;
					case 5:
						xDigitString = "5";
						goto default;
					case 6:
						xDigitString = "6";
						goto default;
					case 7:
						xDigitString = "7";
						goto default;
					case 8:
						xDigitString = "8";
						goto default;
					case 9:
						xDigitString = "9";
						goto default;
					case 10:
						xDigitString = "A";
						goto default;
					case 11:
						xDigitString = "B";
						goto default;
					case 12:
						xDigitString = "C";
						goto default;
					case 13:
						xDigitString = "D";
						goto default;
					case 14:
						xDigitString = "E";
						goto default;
					case 15:
						xDigitString = "F";
						goto default;
					default:
						Serial.DebugWrite(xDigitString);
						break;
				}
			}
		}

	}
}