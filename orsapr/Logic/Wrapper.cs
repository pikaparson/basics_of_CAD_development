using System;
using System.Collections.Generic;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;

namespace Logic
{
    public class Wrapper
    {
        private IKompasAPIObject _kompas;


        public void OpenCad()
        {
            Type t = Type.GetTypeFromProgID("KOMPAS.Application.7");
            _kompas = (IKompasAPIObject)Activator.CreateInstance(t);
            _kompas.Application.Visible = true;
        }

        public IPart7 CreatePart()
        {
            _kompas.Application.Documents.Add(DocumentTypeEnum.ksDocumentPart);
            IKompasDocument3D document3d = (IKompasDocument3D)_kompas.Application.ActiveDocument;
            return document3d.TopPart;
        }

        public ISketch CreateSketch(IPart7 part, string name)
        {
            IModelContainer modelContainer = (IModelContainer)part;
            ISketch sketch = modelContainer.Sketchs.Add();
            sketch.Plane = part.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY];
            sketch.Name = name;
            sketch.Hidden = false;
            sketch.Update();

            return sketch;
        }

        public void CreateRectangle(ISketch sketch, int x, int y, int width, int height)
        {
            IKompasDocument documentSketch = sketch.BeginEdit();
            IKompasDocument2D document2D = (IKompasDocument2D)documentSketch;
            IViewsAndLayersManager viewsAndLayersManager = document2D.ViewsAndLayersManager;
            IView view = viewsAndLayersManager.Views.ActiveView;
            IDrawingContainer drawingContainer = (IDrawingContainer)view;

            IRectangle rectangle = drawingContainer.Rectangles.Add();
            rectangle.Style = (int)Kompas6Constants.ksCurveStyleEnum.ksCSNormal;
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Update();
            sketch.EndEdit();
        }

        public void ExtrudeSketch(ISketch sketch, double depth, string name, bool draftOutward)
        {
            var part = sketch.Part;
            var modelContainer = (IModelContainer)part;
            var extrusions = modelContainer.Extrusions;

            IExtrusion extrusion = extrusions.Add(Kompas6Constants3D.ksObj3dTypeEnum.o3d_bossExtrusion);
            extrusion.Direction = Kompas6Constants3D.ksDirectionTypeEnum.dtNormal;
            extrusion.Name = name;
            extrusion.Hidden = false;
            extrusion.ExtrusionType[true] = Kompas6Constants3D.ksEndTypeEnum.etBlind;
            extrusion.DraftOutward[true] = draftOutward;
            extrusion.DraftValue[true] = 0.0;
            extrusion.Depth[true] = depth;

            IExtrusion1 extrusion1 = (IExtrusion1)extrusion;
            extrusion1.Profile = sketch;
            extrusion1.DirectionObject = sketch;
            extrusion.Update();
        }
    }
}