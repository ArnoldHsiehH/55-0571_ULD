/*************************************************************************/
/*! Project WID110DLL - Wafer-ID Reader Library
*
*   @file  WID110Dll.h
*
*   @brief WID110-dll headerfile
*
*   @author (c) 2008 IOSS GmbH
*
*   @version 1.3.0
*
*   @date 01-06-2008
*/
/*********************************************************************sr**/
#if !defined(AFX_WID110DLL_H__INCLUDED_)
#define AFX_WID110DLL_H__INCLUDED_

#pragma once

#ifdef _WID110LIB_DLLAPI_
#define WID110LIB_DLLAPI  __declspec( dllexport )
#else
#define WID110LIB_DLLAPI  __declspec( dllimport )
#endif

class CLib;

//###########################
// definitions
//###########################

//---------------------------
// Lib error handling
//---------------------------
#define ERROR_NONE                      0           //!< no error
#define ERROR_NOT_INIT                  1           //!< Init() must be executed successfully before lib can be used
#define ERROR_READER_NOTFOUND           2           //!< couldn't connect to specified reader IP
#define ERROR_NETWORK_INIT              3           //!< reader not connected or synchronisation with reader failed
#define ERROR_NETWORK_IP                4           //!< invalid IP
#define ERROR_NETWORK_SEND              5           //!< error while sending messages to reader
#define ERROR_NETWORK_RECEIVE           6           //!< error while receiving messages from reader
#define ERROR_FILENAME                  7           //!< invalid image path and filename
#define ERROR_IMG_SAVE                  8           //!< error saving image to disk
#define ERROR_PARAM_LOAD                9           //!< error while loading parameter
//#define ERROR_NO_PROC_TRIGGERSTR       10           //!< empty string for process triggering found
#define ERROR_ARGBUFFERSIZE            11           //!< argument buffer too small
#define ERROR_NO_FAILSTRING            12           //!< no valid fail string set in WID110 reader; needed for getting read success/failure state
#define ERROR_NO_MORE_IMAGES           13           //!< if GetImage() is called multiple times after StartReadProcess(), this indicates, there are no more images
#define ERROR_NO_VALID_RESULT          14           //!< no valid result retrieved from reader after last trigger command
#define ERROR_NO_VALID_IMAGE           15           //!< no valid image data retrieved from reader after last trigger command
#define ERROR_NO_TRIGGERSTRING         16           //!< no valid trigggerstring is set on WID110 reader, needed for process trigger functionality
#define ERROR_VERSION_PARAMETERSET     17           //!< connected reader runs incompatible versiojn of IOSS software for this lib
#define ERROR_PARAMETER_OUTOFRANGE     18           //!< invalid parameter
#define ERROR_NET_TRIGGERING           19           //!< disabled net trigger on reader, process trigger not possible
#define ERROR_READER_TIMEOUT		   20
#define ERROR_READER_NOT_RUNNING	   21
//---------------------------
// C-function error handling
//---------------------------
#define RETVAL_NO_ERROR                 1           //!< no error
#define RETVAL_ERROR                    0           //!< error occured
#define RETVAL_INV_OBJ                (-1)          //!< invalid object argument


//! channel types for reader lighting in LiveGetImage() and LiveRead()
#define CH_BRIGHTFIELD                  0           //!< brightfield channel
#define CH_FOCUSSED                     1           //!< focussed channel
#define CH_INNER_ROW                    2           //!< inner LED row
#define CH_OUTER_ROW                    3           //!< outer LED row
#define CH_ALLROWS                      4           //!< all channels
#define CH_EXTERNAL                     5           //!< max value for channels
#define CH_MAXVALUE                     6           //!< max value for channels

//! colors for reader lighting in LiveGetImage() and LiveRead()
#define COLOR_RED                       0            //!< red LED color
#define COLOR_GREEN                     1            //!< green LED color
#define COLOR_BLUE                      2            //!< blue LED color
#define COLOR_MAXVALUE                  2            //!< max value for color

//! intensity for reader lighting in LiveGetImage() and LiveRead()
#define INTENSITY_MINVALUE              0            //!< intensity minimal value
#define INTENSITY_MAXVALUE             180          //!< intensity maximal value


//! parameter value for type of processing in ProcessGetImage()
#define IMG_PROCESS_BEST                0            //!< retrieve the successful decoded image only or, in case of an No Read, the one with the highest contrast
#define IMG_PROCESS_ALL                 1            //!< retrieve next subsequent image of a sequence

/**********************************************************************************************//**
 * @struct	WID_CAPTURE
 *
 * @brief	WID120 Grabbing data
 *
 * @author	Jg
 * @date	10.05.2017
 **************************************************************************************************/
typedef struct WID_CAPTURE
{
	int	widColor;			//!< light color		(0..2)
	int	widChannel;			//!< light channel		(0..6)
	int widIntensity;		//!< light intensity	(0..180) 0=off
	int widRotated;			//!< Image Rotated		(0..1)
	int widFlipped;			//!< Image Flipped		(0..1)
}	WID_CAPTURE;

/**********************************************************************************************//**
 * @struct	WID_ROI
 *
 * @brief	A wid region of interest
 *
 * @author	Jg
 * @date	14.06.2017
 **************************************************************************************************/

typedef struct WID_ROI
{
	int	roiXS;					//!< X-Start			(0..960)
	int	roiXL;					//!< X-Length			(64..960)
	int	roiYS;					//!< Y-Start			(0..304)
	int	roiYL;					//!< Y-Length			(64..368)

}	WID_ROI;

/**********************************************************************************************//**
 * @struct	WID_OCR
 *
 * @brief	WID120 OCR parameter
 *
 * @author	Jg
 * @date	10.05.2017
 **************************************************************************************************/

typedef struct WID_OCR
{
	char	widFontName[12];	//!< Name of font				"semi_5x9","semi_org","semi_lin","ocra"
	char	widFielding[32];	//!< Fielding of String			"aannnn-nn-cc"
	char	widFormat  [32];	//!< Fielding of String			"++++++++++++"
	char	widResult  [32];	//!< Result of String			"Reading Result"
	int		widMinSimilarity;	//!< minimum Similarity			(50..75)
	int		widAccSimilarity;	//!< accepted Similarity		(85..99)
	int		widSpacing;			//!< Spacing of Character		(80..200)
	int		widRotation;		//!< Rotation of charactor		(..90..)
	int		widAdjustSpace;		//!< Adjust Spacing				(0..1)
	int		widAdjustSize;		//!< Adjust Size of Characters	(0..1)
	int		widFilter;			//!< Filter						(0..40)
	int		widCharSizeX;		//!< Character Size in X		(15.. 60)
	int		widCharSizeY;		//!< Character Size in Y		(27..108)
	int		widCharPosX;		//!< Character Position in X	(64..960)
	int		widCharPosY;		//!< Character Position in Y	(64..304)
	WID_ROI	widRoi;				//!< Region of Interrest
}	WID_OCR;

/**********************************************************************************************//**
 * @struct	WID_BCR
 *
 * @brief	WID120 Barcode Parameter
 *
 * @author	Jg
 * @date	10.05.2017
 **************************************************************************************************/

typedef struct WID_BCR
{
	int		widIBM412;			//!< Activate IBM BC412			(0..1)
	int		widIBMConversion;	//!< IBM- Conversion			(0..2) None,Base 35,Custom A, Custom V, Custom R, Programmable
	char	widIBMTable[40];	//!< IBM- Conversion Table		("000102030405060708091011121314151517") 
	char	widFormat  [32];	//!< Format of String			"++++++++++++"
	int		widSemi;			//!< Activate Semi Barcode		(0..1)
	int		widSemiConversion;	//!< Semi Conversion			(0..3) None,Base 35, Custom, Custom Checksum
	int		widBarCodeLength;	//!< Code Length				(0..18) 0=auto
	int		widSeparator;		//!< Separator Character		(0..4) none,DASH,POINT,SPACE,HASH
	int		widWaferNumber;		//!< Wafer Number				(0..3) 2 digits, suppress leading zero, 3 digits
	int		widShowCheck;		//!< Show Check Character		(0..1)
	int		widResolution;		//!< Resolution of Barcode		(0..2) Standard, High Resolution, Standard & High Resolution
	int		widLowContrast;		//!< Contrast of Barcode		(0..1) Activate Low Contrast Codes
	WID_ROI	widRoi;				//!< Region of Interrest
}	WID_BCR;

/**********************************************************************************************//**
 * @struct	WID_DMR
 *
 * @brief	WID120 2D-Code Parameter
 *
 * @author	Jg
 * @date	10.05.2017
 **************************************************************************************************/

typedef struct WID_DMR
{
	int		widCodeType;		//!< Activate Code Type			(0..2)	Off, DataMatrix, QR-Code
	int		widCodePlot;		//!< Code Plot Style			(0..3)	standard code,dotted code, standard & dotted code
	int		widCodeMode;		//!< Code Mode (mirrored)		(0..2)	normal, mirrored, normal & mirrored
	int		widCodeSize;		//!< Module Size of Code		(0..3)	large, semi, small, tiny
	char	widFormat[32];		//!< Format of String			"++++++++++++"
	WID_ROI	widRoi;				//!< Region of Interrest
}	WID_DMR;

// 15.08.07		1.0.0	- first version for Fa. SPA
// 10.10.07 	1.0.1	- more loadable configuration file types; enabled process triggering; fixed bug when decoding was switched
// 22.02.08     1.0.2   - decoding success info in GetWaferId()
//                      - synchronisation after connect is retrieving error string and trigger string from reader
//                      - function calls trigger ERROR_NOT_INIT when not connected to reader
//                      - live image without deoding in StartReadImage() (decoding is switched off)
//                      - support for VC++6 und VC++7.1 in separate libs and example implementations for both platforms
// 23.02.08     1.0.3   - enabling process triggering enables process triggering on reader automatically
// 18.03.08     1.0.4   - bugfix in LIB->GetErrorDescription;
//                      - fixed: SocketClose() nur wenn Socket connected ist
// 17.04.08     1.1.0   - compatibility to reader parameter set v5.1.3
// 27.05.08     1.2.0   - compatibility to reader parameter set v5.1.6
// 22.09.08     1.3.0   - DLL version for Visual Studio 6 only, compatibility to reader parameter set v3.4.1
// 21.11.08     1.4.0   - separate versions for Lib (GetVersion()) and WID110 parameter set compatibility (GetVersionParam())
// 07.07.09     1.5.0   - compatibility to reader parameter set v5.6.0
//                      - DMR Trigger possible
// 04.08.09     1.5.1   - compatibility to reader parameter set v5.6.0
//                      - new function StartReadProcess() for triggering a process trigger (no image is retrieved yet after process trigger)
// 06.08.09     1.6.0   - compatibility to reader parameter set v5.7.0
//                      - possible to request code quality
// 10.08.09     1.7.0   - compatibility to reader parameter set v5.7.0
//                      - added a IP address field in example program
//                      - changed: DLL::Init() now takes constant string pointer
// 24.08.09     1.8.0   - compatibility to reader parameter set v5.8.0
// 15.09.09     1.8.1   - switching decoder state in StartReadImage() disabled, because a following StartReadProcess() would fail
// 15.09.09     1.8.2   - disabled automatic reset of overlay in reader after connect, new function SwitchOverlay() implemented
// 16.09.09     1.9.0   - compatibility to reader parameter set v5.9.0 added
//                      - enabled GetImage() functionality after StartReadProcess()
//                      - StartReadImage() delivers a live image without decoding while ignoring the readers decoding state
//                      - StartReadNormal() is replacing all StartReadX() and is doing a panel trigger using all configurations
//                      - more error codes for more control over valid image data and valid result after a trigger
//                      - version release deferred
// 08.10.09     2.0.0   - compatibility to reader parameter set v6.0.x, which is not released yet
//                      - Interface Redesign
//                          - stronger separation between live image and process image "trigger and collection"-functions
//                          - live image collection with parameters for image acquisition and lighting
//                          - possibility to get all process images or just the best image after a trigger
// 19.10.09     2.0.1   - fixed: some example program listboxes had "sort" setting enabled
//                      - lib doesn't upload network parameters if config files (*.cfg) get loaded
// 15.02.10     2.1.0   - fixed: GetWaferId() now throws error ERROR_ARGBUFFERSIZE if result got truncated
//                      - example console application added with project files for VS v6.0 and v9.0
//                      - lib uses batchfiles to distribute def + header files and binaries to example projects
//                      - added support for WID110 parameterset v6.1.x
// 15.02.10     2.1.1   - building of example projects optimized, C#-Version with VS7.1 and example project
// 24.02.10     2.2.0   - changed return type from BOOL to bool to enable inclusion in non mfc projects
//                      - error handling for C-functions updated
//
// 24.03.10     2.2.1   - bugfix: don't send code quality and result preset when configfile is loaded
// 30.03.10     2.3.0   - LiveRead() overloaded, new exported with function FuncLiveGetImageRead()
//                      - loading configs is not overwriting certain values anymore
// 31.03.10     2.4.0   - class function return values changed back to BOOL to keep the lib's exported class names 
//                        compatible to versions previous 2.1.1
//                          - this means projects using the exported class cannot use dll versions 2.2.x and 2.3.x 
//                            with other versions without rebuilding their application  
// 31.03.10     2.4.1   - file type *.fnt in WID110lib and Demo-Programm included
// 01.03.11     2.4.2   - missing socket thread patch is causing memory leaks when closing lib in C++ examples
// 04.07.11     2.5.0   - support for reader version parameter set 6.2.x
// 18.07.11     2.6.0   - support for reader version parameter set 6.3.x
// 18.07.11     2.6.1   - added jobfile support
// 09.11.11     2.7.0   - supports reader param v6.5.x, still compatible to old lib interface
// 09.11.11     2.7.1   - bugfix: process trigger didn't work when no config was loaded before
// 09.11.11     6.5.0   - supports reader param v6.5.x; not compatible to old lib interface;
//                      - version number scheme adapted to reader sw; 
//                      - GetCodeTime() added
// 09.11.11     6.5.1   - supports reader param v6.5.x; changed GetCodeTime() to only getting the measurement time
// 19.07.12     6.5.2   - loading *.wid files does not overwrite lastBest settings anymore
// 17.08.12     6.6.0   - support for reader version parameter set 6.6.x
// 27.02.12     7.0.0   - supports reader param v7.0.x 
// 19.07.12     7.0.1   - loading *.wid files does not overwrite lastBest settings anymore
// 20.07.12     7.1.0   - support for reader version parameter set 7.1.x
// 20.07.12     7.2.0   - support for reader version parameter set 7.2.x
// 24.10.13     7.2.1   - loading/saving reference parameters only if referencing is enabled
// 01.07.2016   8.2.0   - wid120 version for 64 bit / 32 bit
// 06.09.2016   8.2.1   - wid120 version for 64 bit / 32 bit, added 64 bit example versions                 
//                        chg: examples pull dlls and lib files before build
//                        chg: examples copy their sources and binaries to the release folder after build
//                        bug: loadRecipe now throws error when file invalid extension (bug introduced in 8.2.0)
//                        chg: lib projects copies param files from interface foolder before build
// 28.11.2016   8.3.0   - Ergänzungen im Parametersatz
// 28.11.2016   8.4.0   - Ergänzungen im Parametersatz HW_FIRMWARE, Erweiterung Wertebereich für Licht (0..180)
// 05.05.2017   8.4.1   - Ergänzungen um Funktion SetImageOrientation(nRotate,nFlipped)
// 22.05.2017   8.4.2   - Änderung der Funktion SetImageCapture(WID_Capture *pCapture)
//						- Erweiterung auf Funktionen zum Einlernen von OCR-Schriften (BCR und DMR sind dummy-Funktionen)
// 13.08.2017   8.4.5   - Rückgabe von TeachinOCR() muss Erfolg mitteilen
// 16.10.2017	8.4.6	- zu kurzer Buffer bei Stringerzeugung, Anpassungen analog zur JavaLib
// 07.11.2017	8.4.7	- MFC Vollständig aus der Lib entfernt.
// 10.11.2017	8.4.8	- weitere Funktionen nachgeführt
// 13.11.2017	8.4.9   - Funktion zur Überprüfung der Verbindung zur Kamera
// 25.01.2018	8.4.10  - BugFix in network wegen WSACLean() udn WSAStartup() 
// 27.01.2018	8.4.10  - Für CIM-T ist Versionskennung auf 7.2.0 abzuändern, da dort das Programm abgebrochen wird
// 24.04.2018	8.4.11	  CLR-Support aktiviert. Nicht verwendete DLLs enfernt.
// 03.05.2018	8.4.11	  CompileAsManagedCode deaktiviert um die Libary unter alten Visual Basic Versionen nutzen zu können.
// 08.05.2018	8.4.12	  Option "MFC in einer statischen Bibliothek verwenden" aktiviert. Somit ist werden alle benötigten DLLs und LIBs eingebunden und müssen nicht nachinstalliert werden.

//#define WID110LIB_VERSION   ("7.2.0")       //!< WID110 Library Version
#define WID110LIB_VERSION   ("8.4.12")       //!< WID110 Library Version (CIM-T)
#define WID110LIB_REVISION  ("r792")       //!< WID110 Library Revision (CIM-T)


////////////////////////////////////////////////////////////
/*!
*	@class CWID110Dll
*
*	@brief class definition for CWID110Dll
*
*   - if a function returns FALSE,
*       - the error can be retrieved with GetLastError()
*       - errors are reset with next successful function call
*
*
*/
////////////////////////////////////////////////////////////
class WID110LIB_DLLAPI CWID110Dll
{
    public:
    CWID110Dll();
    ~CWID110Dll();

    // control functions
    BOOL  Init(const char *cpIPAddress);
    BOOL  IsInitialized();
    char* GetVersionParam();
    char* GetVersion();
    BOOL  SwitchOverlay(BOOL bOnOff);
    BOOL  Exit();

    // live functions
    BOOL LiveGetImage(const char *cpFileName, int nChannel, int nIntensity, int nColor);
    BOOL LiveRead();
    BOOL LiveRead( const char *cpFileName, int nChannel, int nIntensity, int nColor );

    // process functions
    BOOL ProcessRead();
    BOOL ProcessGetImage(const char *cpFileName, int nTypeImage);

    // evaluation functions
    BOOL GetWaferId( char * cReadId, unsigned int nMaxLen, BOOL *bReadSuccessful);
    BOOL GetCodeQualityOCR(int *pnQuality);
    BOOL GetCodeQualityBCR(int *pnQuality);
    BOOL GetCodeQualityDMR(int *pnQuality);
    BOOL GetCodeQualityLast(int *pnQuality);

    BOOL GetCodeTime(int *pnTime);
    
    // configuration handling functions
    BOOL LoadRecipes(const char *cpFilePath, const char *cpFilename);
    BOOL LoadRecipes(const char *cpFilePath, const char *cpFilename, int nSlot);

    // error handling functions
    int  GetLastError();
    BOOL GetErrorDescription(int nError, char* strText, int nTextLength);
 
    BOOL SaveJob(const char *cpFilePath, const char *cpFilename);

	// image capturing Parameter

    BOOL SetImageCapture(WID_CAPTURE *pCapture);
    BOOL GetImageCapture(WID_CAPTURE *pCapture);

	// Reading Parameter
    BOOL SetParamBCR(WID_BCR *pBCR, WID_CAPTURE *pCapture);
    BOOL GetParamBCR(WID_BCR *pBCR, WID_CAPTURE *pCapture);

    BOOL SetParamOCR(WID_OCR *pOCR, WID_CAPTURE *pCapture);
    BOOL GetParamOCR(WID_OCR *pOCR, WID_CAPTURE *pCapture);

    BOOL SetParamDMR(WID_DMR *pDMR, WID_CAPTURE *pCapture);
    BOOL GetParamDMR(WID_DMR *pDMR, WID_CAPTURE *pCapture);

	int	 TeachingBCR( void );
	int  TeachingOCR( void );
	int  TeachingDMR( void );

	int	 ConfigureBCR		( char *cName);
	int	 ConfigureOCR		( char *cName);
	int	 ConfigureDMR		( char *cName);
	int	 ConfigureDelete	( char *cName);

	int  GetImageRawData ( BYTE *pImageData );
	int  GetImageXSize  ( void );
	int  GetImageYSize  ( void );

	BOOL SetFineScan ( int *pMode );
	BOOL GetFineScan ( int *pMode );

	BOOL GetFontName ( char *fName , int index );
	BOOL GetConfigurationName( char *cName, int index);
	BOOL SaveJobToReader( void );

	BOOL GetROI ( char *cName , WID_ROI *pWnd, WID_CAPTURE *pCapture);
	BOOL SetROI ( char *cName , WID_ROI *pWnd, WID_CAPTURE *pCapture);

	BOOL CheckConnection ( char *cpIPAddress, int timeout);

    private:
    CLib	*m_lib;		        // internal lib object
};
#endif //AFX_WID110DLL_H__INCLUDED_


////////////////////////////////////////////////////////////
/*
*	C-style interface functions for non object oriented
*   programming environments
*
*   - the WIDLib object is created in FuncCreateDll() and a
*     pointer to the object is returned and needs to be stored
*   - the pointer to the lib object needs to be provided in every
*     function call
*   - the lib object can be destroyed by calling FuncDestroyDll()
*/
////////////////////////////////////////////////////////////

// control functions
void *  __stdcall FuncCreateDll();
int     __stdcall FuncDestroyDll(           void * objptr);
int     __stdcall FuncInit(                 void * objptr, char *cpIPAddress);
int     __stdcall FuncIsInitialized(        void * objptr);
int     __stdcall FuncGetVersionParam(      void * objptr, char *cVersion, int nMaxLen);
int     __stdcall FuncGetVersion(           void * objptr, char *cVersion, int nMaxLen);
int     __stdcall FuncSwitchOverlay(        void * objptr, int bOnOff);
int     __stdcall FuncExit(                 void * objptr);
int		__stdcall FuncCheckConnection(		void * objptr, char *cpIPAddress, int timeout);

// exchange parameter functions
int     __stdcall FuncSetImageCapture(     void * objptr, WID_CAPTURE *pCapture);
int     __stdcall FuncGetImageCapture(     void * objptr, WID_CAPTURE *pCapture);
int     __stdcall FuncSetOcrDecoding (     void * objptr, WID_OCR *pOcr);
int     __stdcall FuncGetOcrDecoding (     void * objptr, WID_OCR *pOcr);
int     __stdcall FuncSetBcrDecoding (     void * objptr, WID_BCR *pBcr);
int     __stdcall FuncGetBcrDecoding (     void * objptr, WID_BCR *pBcr);
int     __stdcall FuncSetDmrDecoding (     void * objptr, WID_DMR *pDmr);
int     __stdcall FuncGetDmrDecoding (     void * objptr, WID_DMR *pDmr);

// live functions
int     __stdcall FuncLiveGetImage(         void * objptr, const char *cpFileName, int nChannel, int nIntensity, int nColor);
int     __stdcall FuncLiveRead(             void * objptr);
int     __stdcall FuncLiveGetImageRead(     void * objptr, const char *cpFileName, int nChannel, int nIntensity, int nColor );



// process functions
int     __stdcall FuncProcessRead(          void * objptr);
int     __stdcall FuncProcessGetImage(      void * objptr, const char *cpFileName, int nTypeImage);

// evaluation functions
int     __stdcall FuncGetWaferId(           void * objptr, char * cReadId, int nMaxLen, int *bReadSuccessful);
int     __stdcall FuncGetCodeQualityOCR(    void * objptr, int *pnQuality);
int     __stdcall FuncGetCodeQualityBCR(    void * objptr, int *pnQuality);
int     __stdcall FuncGetCodeQualityDMR(    void * objptr, int *pnQuality);
int     __stdcall FuncGetCodeQualityLast(   void * objptr, int *pnQuality);

int     __stdcall FuncGetCodeTime(          void * objptr, int *pnTime);

// configuration handling functions
int     __stdcall FuncLoadRecipes(          void * objptr, const char *cpFilePath, const char *cpFilename);
int     __stdcall FuncLoadRecipesToSlot(    void * objptr, const char *cpFilePath, const char *cpFilename, int nSlot);

// error handling functions
int     __stdcall FuncGetLastError(         void * objptr);
int     __stdcall FuncGetErrorDescription(  void * objptr, int nError, char* strText, int nTextLength);

// teachin functions

int		__stdcall FuncTeachingBCR  (		void * objptr);			 
int		__stdcall FuncTeachingOCR  (		void * objptr);			 
int		__stdcall FuncTeachingDMR  (		void * objptr);			 

int		__stdcall FuncConfigureBCR		(	void * objptr, const char *cName);
int		__stdcall FuncConfigureOCR		(	void * objptr, const char *cName);
int		__stdcall FuncConfigureDMR		(	void * objptr, const char *cName);
int		__stdcall FuncConfigureDelete	(	void * objptr, const char *cName);

// image handling functions

int		__stdcall FuncGetImageRawData	(	void * objptr, char *pImageData);
int		__stdcall FuncGetImageXSize		(	void * objptr);
int		__stdcall FuncGetImageYSize		(	void * objptr);

// further function for change configuration

int		__stdcall FuncSetFineScan		(	void * objptr, int *pMode );
int		__stdcall FuncGetFineScan		(	void * objptr, int *pMode );

int		__stdcall FuncGetFontName		( void * objptr, char *fName , int index );
int		__stdcall FuncGetConfigurationName( void * objptr, char *cName, int index );
int		__stdcall FuncSaveJobToReader	( void * objptr );

int		__stdcall FuncGetROI			(	void * objptr, char *cName , WID_ROI *pWnd, WID_CAPTURE *pCapture);
int		__stdcall FuncSetROI			(	void * objptr, char *cName , WID_ROI *pWnd, WID_CAPTURE *pCapture);

