using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace orsapr.Wrapper
{
    public class Wrapper
    {
        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        public void Build(Parameters parameters)
        {
            // Open cad
            Type t = Type.GetTypeFromProgID("KOMPAS.Application.7");
            IKompasAPIObject kompas7 = (IKompasAPIObject)Activator.CreateInstance(t);
            kompas7.Application.Visible = true;

            // Create part
            kompas7.Application.Documents.Add(DocumentTypeEnum.ksDocumentPart);
            IKompasDocument3D document3d = (IKompasDocument3D)kompas7.Application.ActiveDocument;
            IPart7 part = document3d.TopPart;
            IModelContainer modelContainer = (IModelContainer)part;

            // Extrusions collection
            IExtrusions extrusions = modelContainer.Extrusions;

            // Create seat
            ISketch sketch = modelContainer.Sketchs.Add();
            sketch.Plane = part.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY];
            sketch.Name = "Эскиз: Сидушка";
            sketch.Hidden = false;
            sketch.Update();

            IKompasDocument documentSketch = sketch.BeginEdit();
            IKompasDocument2D document2D = (IKompasDocument2D)documentSketch;
            IViewsAndLayersManager viewsAndLayersManager = document2D.ViewsAndLayersManager;
            IView view = viewsAndLayersManager.Views.ActiveView;
            IDrawingContainer drawingContainer = (IDrawingContainer)view;

            IRectangle rectangle = drawingContainer.Rectangles.Add();
            rectangle.Style = (int)Kompas6Constants.ksCurveStyleEnum.ksCSNormal;
            rectangle.Width = parameters.SeatWidth;
            rectangle.Height = parameters.SeatLength;
            rectangle.Update();
            sketch.EndEdit();

            IExtrusion extrusion = extrusions.Add(Kompas6Constants3D.ksObj3dTypeEnum.o3d_bossExtrusion);
            extrusion.Direction = Kompas6Constants3D.ksDirectionTypeEnum.dtNormal;
            extrusion.Name = "Элемент выдавливания: Сидушка";
            extrusion.Hidden = false;
            extrusion.ExtrusionType[true] = Kompas6Constants3D.ksEndTypeEnum.etBlind;
            extrusion.DraftOutward[true] = false;
            extrusion.DraftValue[true] = 0.0;
            extrusion.Depth[true] = parameters.SeatThickness;

            IExtrusion1 extrusion1 = (IExtrusion1)extrusion;
            extrusion1.Profile = sketch;
            extrusion1.DirectionObject = sketch;
            extrusion.Update();

            // Create legs
            List<Point> coords = new List<Point>() { new Point(0, 0), 
                new Point(parameters.SeatWidth - parameters.LegWidth, 0),
                new Point(0, parameters.SeatLength - parameters.LegWidth),
                new Point(parameters.SeatWidth - parameters.LegWidth, parameters.SeatLength - parameters.LegWidth)};

            int legNumber = 0;

            // It seems like kompas cant extrude all legs at once, so we draw one leg and extrude it in cicle
            foreach (var point in coords)
            {
                legNumber++;

                ISketch sketch2 = modelContainer.Sketchs.Add();
                sketch2.Plane = part.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY];
                sketch2.Name = "Эскиз: Ножка " + legNumber;
                sketch2.Hidden = false;
                sketch2.Update();

                IKompasDocument documentSketch2 = sketch2.BeginEdit();
                IKompasDocument2D document2D2 = (IKompasDocument2D)documentSketch2;
                IViewsAndLayersManager viewsAndLayersManager2 = document2D2.ViewsAndLayersManager;
                IView view2 = viewsAndLayersManager2.Views.ActiveView;
                IDrawingContainer drawingContainer2 = (IDrawingContainer)view2;

                IRectangle rectangleLeg = drawingContainer2.Rectangles.Add();
                rectangleLeg.Style = (int)Kompas6Constants.ksCurveStyleEnum.ksCSNormal;
                rectangleLeg.X = point.X;
                rectangleLeg.Y = point.Y;
                rectangleLeg.Width = parameters.LegWidth;
                rectangleLeg.Height = parameters.LegWidth;
                rectangleLeg.Update();
                sketch2.EndEdit();

                IExtrusion extrusionLegs = extrusions.Add(Kompas6Constants3D.ksObj3dTypeEnum.o3d_bossExtrusion);
                extrusionLegs.Direction = Kompas6Constants3D.ksDirectionTypeEnum.dtNormal;
                extrusionLegs.Name = "Элемент выдавливания: Ножка " + legNumber;
                extrusionLegs.Hidden = false;
                extrusionLegs.ExtrusionType[true] = Kompas6Constants3D.ksEndTypeEnum.etBlind;
                extrusionLegs.DraftOutward[true] = false;
                extrusionLegs.DraftValue[true] = 0.0;
                extrusionLegs.Depth[true] = -parameters.LegLength; // it is important - minus (other direction than seat was extruded)

                IExtrusion1 extrusion2 = (IExtrusion1)extrusionLegs;
                extrusion2.Profile = sketch2;
                extrusion2.DirectionObject = sketch2;
                extrusionLegs.Update();
            }

        }
    }
}
