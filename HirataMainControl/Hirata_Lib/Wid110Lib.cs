// ============================================================================
// Copyright (c) 2010  IOSS GmbH
// All Rights Reserved.
// ============================================================================


// ============================================================================
//
//      Wid110Lib  -  WID110 C# based library
//
// ============================================================================
//
//      File:     Wid110Lib.cs               Type:         Implementation
//
//      Date:     05.02.2010                 Last Change:  09.11.2011
//
//      Author:   Thomas M. Schlageter
//                Silvio Robel
//
//      Methods:  Wid110Lib             -  constructor
//                ~Wid110Lib            -  destructor
//
//                FCreateDll            -  create WID110Lib instance
//                FDestroyDll           -  destroy WID110Lib instance
//                FExit                 -  terminate connection and exit
//                FGetCodeQualityBCR    -  get code qualitiy for BCR codes
//                FGetCodeQualityDMR    -  get code qualitiy for DMR codes
//                FGetCodeQualityLast   -  get code qualitiy for LAST code
//                FGetCodeQualityOCR    -  get code qualitiy for OCR codes
//                FGetErrorDescription  -  get error description
//                FGetLastError         -  get the last error number
//                FGetVersionParam      -  return sensor/interface version
//                FGetVersion           -  return library version
//                FGetWaferId           -  get the last BCR/OCR decode result
//                FInit                 -  initialize library and connect
//                FIsInitialized        -  check for initialized state
//                FLiveGetImage         -  take single image with parameters
//                FLiveRead             -  perform a live read
//                FLoadRecipes          -  load parameters by sending a file
//                FLoadRecipesToSlot    -  load parameters by sending a file
//                FProcessGetImage      -  get image from process trigger
//                FProcessRead          -  perform a process read
//                FSwitchOverlay        -  switch overlay on/off
//				  FGetCodeTime		    -  get certain time parameter
//
//
//      Auxiliary methods:
//
//                getErrno              -  get error number
//                getLastExcp           -  get the last exception message
//                getReadOK             -  get result read state
//                getTmpImage           -  return temporary image name
//                isException           -  return exception state
//
// ============================================================================


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

using Wid110LibConstUser;


namespace Wid110LibUser
{
    public class Wid110Lib
    {
        // ====================================================================
        /// <summary>
        /// Private member variables.
        /// </summary>
        // ====================================================================

        private string version;
        private string versParam;

        private string tmpImage = Wid110LibConst.tmpImage;
        private string lastExcp;

        private int     errno  = Wid110LibConst.ecNone;
        private int     readOK = Wid110LibConst.rcError;

        private IntPtr dll;


        // ====================================================================
        /// <summary>
        /// Imported method prototypes.
        /// </summary>
        // ====================================================================

        [DllImport( "wid110Lib.dll" )]
        public static extern IntPtr FuncCreateDll();

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncDestroyDll( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncInit( IntPtr objptr,
                                           string cpIPAddress );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncIsInitialized( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetVersionParam( IntPtr        objptr,
                                                      StringBuilder cVersion,
                                                      int           nMaxLen );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetVersion( IntPtr        objptr,
                                                 StringBuilder cVersion,
                                                 int           nMaxLen );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncSwitchOverlay( IntPtr objptr,
                                                    int    bOnOff );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncExit( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLiveGetImage( IntPtr objptr,
                                                   string cpFileName,
                                                   int    nChannel,
                                                   int    nIntensity,
                                                   int    nColor );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLiveRead( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLiveGetImageRead(  IntPtr objptr,
                                                        string cpFileName,
                                                        int    nChannel,
                                                        int    nIntensity,
                                                        int    nColor );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncProcessRead( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncProcessGetImage( IntPtr objptr,
                                                      string cpFileName,
                                                      int    nTypeImage );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetWaferId( IntPtr        objptr,
                                                 StringBuilder cReadId,
                                                 int           nMaxLen,
                                                 IntPtr        bReadSuccessful );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetCodeQualityOCR( IntPtr objptr,
                                                        IntPtr pnQuality );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetCodeQualityBCR( IntPtr objptr,
                                                        IntPtr pnQuality );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetCodeQualityDMR( IntPtr objptr,
                                                        IntPtr pnQuality );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetCodeQualityLast( IntPtr objptr,
                                                         IntPtr pnQuality );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetCodeTime( 		 IntPtr objptr,
														 IntPtr pnTime    );
														 
        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLoadRecipes( IntPtr objptr,
                                                  int    nTimeType,
                                                  IntPtr pnTime );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLoadRecipes( IntPtr objptr,
                                                  string cpFilePath,
                                                  string cpFilename );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncLoadRecipesToSlot( IntPtr objptr,
                                                        string cpFilePath,
                                                        string cpFilename,
                                                        int    nSlot );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncSaveJob(	IntPtr objptr,
                                                string cpFilePath,
                                                string cpFilename );

        [DllImport("wid110Lib.dll")]
        public static extern int FuncSetImageOrientation(IntPtr objptr,
                                                int nFlippied,
                                                int nRotated);
                                                        
        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetLastError( IntPtr objptr );

        [DllImport( "wid110Lib.dll" )]
        public static extern int FuncGetErrorDescription( IntPtr        objptr,
                                                          int           nError,
                                                          StringBuilder strText,
                                                          int           nTextLength );


        // ====================================================================
        /// <summary>
        /// Constructor: Initialize WID C# Library.
        /// </summary>
        // ====================================================================

        public Wid110Lib()
        {
          dll = FCreateDll();
        }


        // ====================================================================
        /// <summary>
        /// Destructor: Remove WID C# Library.
        /// </summary>
        // ====================================================================

        ~Wid110Lib()
        {
          FDestroyDll( dll );
        }


        // ====================================================================
        /// <summary>
        /// Create WID C# Library instance.
        /// </summary>
        /// <return> DLL if created, NULL upon error.</return>
        // ====================================================================

        private IntPtr FCreateDll()
        {
          IntPtr d = IntPtr.Zero;

          lastExcp = "";
          errno    = Wid110LibConst.ecNone;

          try
          {
            d = FuncCreateDll();
          }

          catch ( Exception e )
          {
            lastExcp = e.ToString();
          }

          return d;
        }


        // ====================================================================
        /// <summary>
        /// Destroy WID C# Library instance.
        /// </summary>
        /// <param name="dll"> DLL to destroy.</param>
        /// <return>           true if destroyed.</return>
        // ====================================================================

        private bool FDestroyDll( IntPtr dll )
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch( FuncDestroyDll( dll ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Initialize library and connect to IP.
        /// </summary>
        /// <param name="ip"> IP to connect.</param>
        /// <return>          true if connected.</return>
        // ====================================================================

        public bool FInit( string ip )
        {
            bool isInit = false;

            errno       = Wid110LibConst.ecNone;
            lastExcp    = "";

            try
            {
                switch ( FuncInit( dll, ip ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        isInit = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }


            return isInit;
        }


        // ====================================================================
        /// <summary>
        /// Check library state.
        /// </summary>
        /// <return> true if initialized.</return>
        // ====================================================================

        public bool FIsInitialized()
        {
            if (dll == IntPtr.Zero)
            {
                errno  = Wid110LibConst.ecInvObj;
                return false;
            }

            bool isInit = false;

            errno       = Wid110LibConst.ecNone;
            lastExcp    = "";

            try
            {
                switch ( FuncIsInitialized(dll) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        isInit = true;
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return isInit;
        }


        // ====================================================================
        /// <summary>
        /// Terminate connection.
        /// </summary>
        /// <return> true if disconnected.</return>
        // ====================================================================

        public bool FExit()
        {
            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncExit( dll ) )
                {
                    case Wid110LibConst.rcError:
                    case Wid110LibConst.rcNoError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return true;	// if FuncExit() fails or not, lib is always "not init"
        }


        // ====================================================================
        /// <summary>
        /// Get library version.
        /// </summary>
        /// <param name="v"> WID110 library version.</param>
        // ====================================================================

        public string FGetVersion()
        {
            StringBuilder sb = new StringBuilder( "", Wid110LibConst.versLenCS );

            errno            = Wid110LibConst.ecNone;
            lastExcp         = "";

            try
            {
                switch ( FuncGetVersion( dll, sb, Wid110LibConst.versLenC ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        version = "";		// reset string if error occured
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        version = sb.ToString();	// copy string if lib function had no error
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return version;
        }


        // ====================================================================
        /// <summary>
        /// Get sensor interface version.
        /// </summary>
        /// <return> WID110 interface version.</return>
        // ====================================================================

        public string FGetVersionParam()
        {
            StringBuilder sb = new StringBuilder( "", Wid110LibConst.versLenCS );

            errno            = Wid110LibConst.ecNone;
            lastExcp         = "";

            try
            {
                switch ( FuncGetVersionParam( dll, sb, Wid110LibConst.versLenC ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        versParam = "";		// reset string if error occured
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        versParam = sb.ToString();	// copy string if lib function had no error
                        break;
                    }
                }
            }
            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return versParam;
        }


        // ====================================================================
        /// <summary>
        /// Change overlay flag.
        /// </summary>
        /// <param name="o"> overlay flag.</param>
        /// <return>         true if changed.</return>
        // ====================================================================

        public bool FSwitchOverlay( bool o )
        {
            bool   ok = false;
            int   ovl = ( o ) ? 1 : 0;

            errno     = Wid110LibConst.ecNone;
            lastExcp  = "";

            try
            {
                switch ( FuncSwitchOverlay( dll, ovl ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Perform a live read using the temporary settings from an
        /// earlier 'FLiveGetImage()' call.
        /// </summary>
        /// <return> true if done.</return>
        // ====================================================================

        public bool FLiveRead()
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncLiveRead( dll ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Perform a live read using the given parameters, and decode.
        /// </summary>
        /// <param name="name">      name to save image to.</param>
        /// <param name="channel">   illumination channel.</param>
        /// <param name="intensity"> illumination intensity.</param>
        /// <param name="color">     illumination color.</param>
        /// <return>                 true if done.</return>
        // ====================================================================

        public bool FLiveRead( string name,
                               int    channel,
                               int    intensity,
                               int    color )
        {
          bool ok  = false;

          errno    = Wid110LibConst.ecNone;
          lastExcp = "";

          try
          {
            switch ( FuncLiveGetImageRead( dll, name, channel, intensity, color ) )
            {
              case Wid110LibConst.rcError:
                break;

              case Wid110LibConst.rcInvObj:
                errno = Wid110LibConst.ecInvObj;
                break;

              case Wid110LibConst.rcNoError:
                ok = true;
                break;
            }
          }

          catch ( Exception e )
          {
            lastExcp = e.ToString();
          }

          return ok;
        }


        // ====================================================================
        /// <summary>
        /// Perform a process read..
        /// </summary>
        /// <return> true if done.</return>
        // ====================================================================

        public bool FProcessRead()
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncProcessRead( dll ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Get code quality for OCR codes.
        /// </summary>
        /// <return> code quality, or rsltNoCodeQuality upon failure.</return>
        // ====================================================================

        public int FGetCodeQualityOCR()
        {
            IntPtr  q = Marshal.AllocHGlobal( sizeof( int ) );
            int    rc = Wid110LibConst.rsltNoCodeQuality;

            errno     = Wid110LibConst.ecNone;
            lastExcp  = "";

            try
            {
                switch (FuncGetCodeQualityOCR( dll, q ))
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        rc = Marshal.ReadInt32(q);
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal( q );

            return rc;
        }


        // ====================================================================
        /// <summary>
        /// Get code quality for BCR codes.
        /// </summary>
        /// <return> code quality, or rsltNoCodeQuality upon failure.</return>
        // ====================================================================

        public int FGetCodeQualityBCR()
        {
            IntPtr q = Marshal.AllocHGlobal(sizeof(int));
            int   rc = Wid110LibConst.rsltNoCodeQuality;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncGetCodeQualityBCR(dll, q) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        rc = Marshal.ReadInt32(q);
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal(q);

            return rc;
        }


        // ====================================================================
        /// <summary>
        /// Get code quality for DMR codes.
        /// </summary>
        /// <return> code quality, or rsltNoCodeQuality upon failure.</return>
        // ====================================================================

        public int FGetCodeQualityDMR()
        {
            IntPtr q = Marshal.AllocHGlobal(sizeof(int));
            int   rc = Wid110LibConst.rsltNoCodeQuality;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncGetCodeQualityDMR(dll, q) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        rc = Marshal.ReadInt32(q);
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal(q);

            return rc;
        }


        // ====================================================================
        /// <summary>
        /// Get code quality for LAST code.
        /// </summary>
        /// <return> code quality, or rsltNoCodeQuality upon failure.</return>
        // ====================================================================

        public int FGetCodeQualityLast()
        {
            IntPtr q = Marshal.AllocHGlobal( sizeof( int ) );
            int   rc = Wid110LibConst.rsltNoCodeQuality;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncGetCodeQualityLast( dll, q ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        rc = Marshal.ReadInt32(q);
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal( q );

            return rc;
        }


        // ====================================================================
        /// <summary>
        /// Retrieve image from last process trigger.
        /// </summary>
        /// <param name="name"> file name to save image.</param>
        /// <param name="type"> 'pvImgBest' or 'pvImgAll'.</param>
        /// <return>            true if OK.</return>
        // ====================================================================

        public bool FProcessGetImage( string name,
                                      int    type )
        {
            bool ok     = false;

            errno       = Wid110LibConst.ecNone;
            lastExcp    = "";

            try
            {
                switch ( FuncProcessGetImage(dll, name, type ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Get the last BCR/OCR/DMR decode result.
        /// </summary>
        /// <return> the latest read result.</return>
        // ====================================================================

        public string FGetWaferId()
        {
            StringBuilder sb = new StringBuilder( Wid110LibConst.rsltERROR,
                                                  Wid110LibConst.rsltLenCS );
            string res  = Wid110LibConst.rsltERROR;
            IntPtr pok  = Marshal.AllocHGlobal( sizeof( int ) );

            readOK      = Wid110LibConst.rcError;
            errno       = Wid110LibConst.ecNone;
            lastExcp    = "";

            try
            {
                switch ( FuncGetWaferId( dll, sb, Wid110LibConst.rsltLenC, pok ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        readOK = Marshal.ReadInt32( pok );
                        
//Mike 29190624
#if true
                        res = sb.ToString();
#else


                        if (1 == readOK )
                          res = Wid110LibConst.rsltREAD + sb.ToString();
                        else
                          res = Wid110LibConst.rsltNOREAD + sb.ToString();
#endif
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal( pok );

            return res;
        }


        // ====================================================================
        /// <summary>
        /// Take a single image using the given parameters.
        /// </summary>
        /// <param name="name">      name to save image to.</param>
        /// <param name="channel">   illumination channel.</param>
        /// <param name="intensity"> illumination intensity.</param>
        /// <param name="color">     illumination color.</param>
        /// <return>                 true if OK.</return>
        // ====================================================================

        public bool FLiveGetImage( string name,
                                   int    channel,
                                   int    intensity,
                                   int    color )
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncLiveGetImage( dll, name, channel, intensity, color ))
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {   ok = true;
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            return ok;
        }

        // ====================================================================
        /// <summary>
        /// Saves reader configuration into a *.wid file on the pc
        /// </summary>
        /// <param name="path"> path to file.</param>
        /// <param name="file"> file name to use.</param>
        /// <return>            true if OK.</return>
        // ====================================================================

        public bool FSaveJob(   string path,
                                string file)
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncSaveJob(dll, path, file) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return ok;
        }


        // ====================================================================
        /// <summary>
        /// Load process parameter file
        /// </summary>
        /// <param name="path"> path to file.</param>
        /// <param name="file"> file to use.</param>
        /// <return>            true if OK.</return>
        // ====================================================================

        public bool FLoadRecipes( string path,
                                  string file)
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncLoadRecipes(dll, path, file) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return ok;
        }



        // ====================================================================
        /// <summary>
        /// Load parameters to a certain parameter slot (if applicable)
        /// </summary>
        /// <param name="path"> path to file.</param>
        /// <param name="file"> file to use.</param>
        /// <param name="slot"> slot (only for ocf and led files).</param>
        /// <return>            true if valid.</return>
        // ====================================================================

        public bool FLoadRecipesToSlot( string path,
                                        string file,
                                        int slot)
        {
            bool ok  = false;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";
            string sPath = path;
            if(false == sPath.EndsWith("\\"))
            {   sPath += ("\\");      
            }
            
            try
            {
                switch ( FuncLoadRecipesToSlot( dll, sPath, file, slot ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        ok = true;
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return ok;
        }

		
        // ====================================================================
        /// <summary>
        /// Get code time parameter
        /// </summary>
        /// <return> overall process time or rsltNoCodeTime upon failure.</return>
        // ====================================================================

        public int FGetCodeTime()
        {
            IntPtr q = Marshal.AllocHGlobal( sizeof( int ) );
            int   rc = Wid110LibConst.rsltNoCodeTime;

            errno    = Wid110LibConst.ecNone;
            lastExcp = "";

            try
            {
                switch ( FuncGetCodeTime( dll, q ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        rc = Marshal.ReadInt32(q);
                        break;
                    }
                }
            }

            catch ( Exception e )
            {
                lastExcp = e.ToString();
            }

            Marshal.FreeHGlobal( q );

            return rc;
        }
		

        // ====================================================================
        /// <summary>
        /// Get error description.
        /// </summary>
        /// <param name="eno"> error number to translate.</param>
        /// <return> error description.</return>
        // ====================================================================

        public string FGetErrorDescription(int eno)
        {
            StringBuilder sb = new StringBuilder( Wid110LibConst.errDESC,
                                                  Wid110LibConst.errLen);
            string text = Wid110LibConst.errDESC;
            int     len = Wid110LibConst.errLen;

            errno       = Wid110LibConst.ecNone;
            lastExcp    = "";

            try		// if lib handle is invalid, error text is generated as well
            {
                switch ( FuncGetErrorDescription( dll, eno, sb, len ) )
                {
                    case Wid110LibConst.rcError:
                    {
                        break;
                    }
                    case Wid110LibConst.rcInvObj:
                    {
                        errno = Wid110LibConst.ecInvObj;
                        break;
                    }
                    case Wid110LibConst.rcNoError:
                    {
                        text = sb.ToString();
                        break;
                    }
                }
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return text;
        }



        // ====================================================================
        /// <summary>
        /// Get last error number.
        /// </summary>
        /// <return> last error number.</return>
        // ====================================================================

        public int FGetLastError()
        {
            if (Wid110LibConst.ecInvObj == errno)	// if lib handle was invalid in last call
            {						// it is probably still invalid, so
                return errno;				// FuncGetLastError() will fail as well
            }

            lastExcp = "";

            try
            {
                errno = FuncGetLastError(dll);		// get last internal lib error
            }

            catch (Exception e)
            {
                lastExcp = e.ToString();
            }

            return errno;
        }



        // ====================================================================
        /// <summary>
        /// Get the last error number.
        /// </summary>
        /// <return> last error number.</return>
        // ====================================================================

        public int getErrno()
        {
          return errno;
        }


        // ====================================================================
        /// <summary>
        /// Get the result read state.
        /// </summary>
        /// <return> result read state.</return>
        // ====================================================================

        public int getReadOK()
        {
          return readOK;
        }


        // ====================================================================
        /// <summary>
        /// Get the last exception message.
        /// </summary>
        /// <return> last exception message.</return>
        // ====================================================================

        public string getLastExcp()
        {
          return lastExcp;
        }


        // ====================================================================
        /// <summary>
        /// Get temporary image name.
        /// </summary>
        /// <return> temporary image name.</return>
        // ====================================================================

        public string getTmpImage()
        {
          return tmpImage;
        }


        // ====================================================================
        /// <summary>
        /// Get exception state.
        /// </summary>
        /// <return> true if exception.</return>
        // ====================================================================

        public bool isException()
        {
          return lastExcp.Length != 0;
        }

    }
}
