//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Image Stitching.
    /// </summary>
    public partial class Stitcher : SharedPtrObject
    {
        /// <summary>
        /// The stitcher statis
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Ok.
            /// </summary>
            Ok = 0,
            /// <summary>
            /// Error, need more images.
            /// </summary>
            ErrNeedMoreImgs = 1,
            /// <summary>
            /// Error, homography estimateion failed.
            /// </summary>
            ErrHomographyEstFail = 2,
            /// <summary>
            /// Error, camera parameters adjustment failed.
            /// </summary>
            ErrCameraParamsAdjustFail = 3
        }

        /// <summary>
        /// Wave correction kind
        /// </summary>
        public enum WaveCorrectionType
        {
            /// <summary>
            /// horizontal
            /// </summary>
            Horiz,
            /// <summary>
            /// Vertical
            /// </summary>
            Vert
        }

        /// <summary>
        /// Stitch mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Mode for creating photo panoramas. Expects images under perspective transformation and projects resulting pano to sphere.
            /// </summary>
            Panorama = 0,

            /// <summary>
            /// Mode for composing scans. Expects images under affine transformation does not compensate exposure by default.
            /// </summary>
            Scans = 1,

        }



        /// <summary>
        /// Creates a Stitcher configured in one of the stitching modes.
        /// </summary>
        /// <param name="mode">Scenario for stitcher operation. This is usually determined by source of images to stitch and their transformation. </param>

        public Stitcher(Mode mode = Mode.Panorama)
        {
            _ptr = StitchingInvoke.cveStitcherCreate(mode, ref _sharedPtr);
        }

        /// <summary>
        /// Compute the panoramic images given the images
        /// </summary>
        /// <param name="images">The input images. This can be, for example, a VectorOfMat</param>
        /// <param name="pano">The panoramic image</param>
        /// <returns>The stitching status</returns>
        public Status Stitch(IInputArray images, IOutputArray pano)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (OutputArray oaPano = pano.GetOutputArray())
                return StitchingInvoke.cveStitcherStitch(_ptr, iaImages, oaPano);
        }

        public int EstimateTransform(IInputArrayOfArrays images, Rectangle[][] rois = null)
        {
            using (InputArray iaImages = images.GetInputArray())
                if (rois == null)
                {
                    return StitchingInvoke.cveStitcherEstimateTransform1(_ptr, iaImages);
                }
                else
                {
                    using (VectorOfVectorOfRect vvr = new VectorOfVectorOfRect(rois))
                    {
                        return StitchingInvoke.cveStitcherEstimateTransform2(_ptr, iaImages, vvr);
                    }
                }

        }

        public int ComposePanorama(IOutputArray pano)
        {
            using (OutputArray oaPano = pano.GetOutputArray())
            {
                return StitchingInvoke.cveStitcherComposePanorama1(_ptr, oaPano);
            }
        }

        public int ComposePanorama(IInputArrayOfArrays images, IOutputArray pano)
        {
            using (InputArray iaImages = images.GetInputArray())
            using (OutputArray oaPano = pano.GetOutputArray())
                return StitchingInvoke.cveStitcherComposePanorama2(_ptr, iaImages, oaPano);
        }

        /// <summary>
        /// Set the features finder for this stitcher.
        /// </summary>
        /// <param name="finder">The features finder</param>
        public void SetFeaturesFinder(Features2D.Feature2D finder)
        {
            StitchingInvoke.cveStitcherSetFeaturesFinder(_ptr, finder.Feature2DPtr);
        }

        /// <summary>
        /// Set the warper creator for this stitcher.
        /// </summary>
        /// <param name="warperCreator">The warper creator</param>
        public void SetWarper(WarperCreator warperCreator)
        {
            StitchingInvoke.cveStitcherSetWarper(_ptr, warperCreator.WarperCreatorPtr);
        }

        /// <summary>
        /// Set the blender for this stitcher
        /// </summary>
        /// <param name="blender">The blender</param>
        public void SetBlender(Blender blender)
        {
            StitchingInvoke.cveStitcherSetBlender(_ptr, blender.BlenderPtr);
        }

        /// <summary>
        /// Get or Set a flag to indicate if the stitcher should apply wave correction
        /// </summary>
        public bool WaveCorrection
        {
            get { return StitchingInvoke.cveStitcherGetWaveCorrection(_ptr); }
            set { StitchingInvoke.cveStitcherSetWaveCorrection(_ptr, value); }
        }

        /// <summary>
        /// The wave correction type.
        /// </summary>
        public WaveCorrectionType WaveCorrectionKind
        {
            get { return StitchingInvoke.cveStitcherGetWaveCorrectionKind(_ptr); }
            set { StitchingInvoke.cveStitcherSetWaveCorrectionKind(_ptr, value); }
        }

        /// <summary>
        /// Get or set the pano confidence threshold
        /// </summary>
        public double PanoConfidenceThresh
        {
            get { return StitchingInvoke.cveStitcherGetPanoConfidenceThresh(_ptr); }
            set { StitchingInvoke.cveStitcherSetPanoConfidenceThresh(_ptr, value); }
        }

        /// <summary>
        /// Get or Set the compositing resolution
        /// </summary>
        public double CompositingResol
        {
            get { return StitchingInvoke.cveStitcherGetCompositingResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetCompositingResol(_ptr, value); }
        }

        /// <summary>
        /// Get or Set the seam estimation resolution
        /// </summary>
        public double SeamEstimationResol
        {
            get { return StitchingInvoke.cveStitcherGetSeamEstimationResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetSeamEstimationResol(_ptr, value); }
        }

        /// <summary>
        /// Get or set the registration resolution
        /// </summary>
        public double RegistrationResol
        {
            get { return StitchingInvoke.cveStitcherGetRegistrationResol(_ptr); }
            set { StitchingInvoke.cveStitcherSetRegistrationResol(_ptr, value); }
        }

        /// <summary>
        /// Release memory associated with this stitcher
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                StitchingInvoke.cveStitcherRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Entry points to the Open CV Stitching module.
    /// </summary>
    public static partial class StitchingInvoke
    {

        static StitchingInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStitcherCreate(
            Stitcher.Mode model,
            ref IntPtr sharedPtr
           );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.Status cveStitcherStitch(IntPtr stitcherWrapper, IntPtr images, IntPtr pano);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetFeaturesFinder(IntPtr stitcherWrapper, IntPtr finder);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWarper(IntPtr stitcher, IntPtr creator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetBlender(IntPtr stitcher, IntPtr b);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherRelease(ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWaveCorrection(
            IntPtr stitcher,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool flag);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStitcherGetWaveCorrection(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetWaveCorrectionKind(IntPtr stitcher, Stitcher.WaveCorrectionType kind);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern Stitcher.WaveCorrectionType cveStitcherGetWaveCorrectionKind(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetPanoConfidenceThresh(IntPtr stitcher, double confThresh);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetPanoConfidenceThresh(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetCompositingResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetCompositingResol(IntPtr stitcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]

        internal static extern void cveStitcherSetSeamEstimationResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetSeamEstimationResol(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStitcherSetRegistrationResol(IntPtr stitcher, double resolMpx);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveStitcherGetRegistrationResol(IntPtr stitcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveStitcherEstimateTransform1(IntPtr stitcher, IntPtr images);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveStitcherEstimateTransform2(IntPtr stitcher, IntPtr images, IntPtr rois);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveStitcherComposePanorama1(IntPtr stitcher, IntPtr pano);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveStitcherComposePanorama2(IntPtr stitcher, IntPtr images, IntPtr pano);
    }
}
