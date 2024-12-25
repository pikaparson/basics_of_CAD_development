using System;
using System.Collections.Generic;
using KompasAPI7;
using Kompas6Constants;
using Kompas6Constants3D;

namespace API_singly
{
    /// <summary>
    /// Обертка для взаимодействия с API Kompas
    /// </summary>
    public class Wrapper
    {
        /// <summary>
        /// Объект API для работы с Kompas
        /// </summary>
        private IKompasAPIObject _kompas;

        public bool IsKompasOpened()
        {
            return _kompas != null;
        }

        /// <summary>
        /// Метод для открытия CAD-приложения
        /// </summary>
        public void OpenCad()
        {
            Type t = Type.GetTypeFromProgID("KOMPAS.Application.7");
            _kompas = (IKompasAPIObject)Activator.CreateInstance(t);
            _kompas.Application.Visible = true;
        }

        /// <summary>
        /// Метод для создания части в 3D документе
        /// </summary>
        /// <returns>Возвращает созданную часть</returns>
        public IPart7 CreatePart()
        {
            _kompas.Application.Documents.Add(DocumentTypeEnum.ksDocumentPart);
            IKompasDocument3D document3d = (IKompasDocument3D)_kompas.Application.ActiveDocument;
            return document3d.TopPart;
        }

        /// <summary>
        /// Метод для создания эскиза на заданной части
        /// </summary>
        /// <param name="part">Часть, к которой добавляется эскиз</param>
        /// <param name="name">Имя создаваемого эскиза</param>
        /// <returns>Созданный эскиз</returns>
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

        /// <summary>
        /// Метод для создания прямоугольника в эскизе
        /// </summary>
        /// <param name="sketch">Эскиз, в который добавляется прямоугольник</param>
        /// <param name="x">Координата X начальной точки</param>
        /// <param name="y">Координата Y начальной точки</param>
        /// <param name="width">Ширина прямоугольника</param>
        /// <param name="height">Высота прямоугольника</param>
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

        /// <summary>
        /// Метод для создания круга в эскизе
        /// </summary>
        /// <param name="sketch">Эскиз, в который добавляется круг</param>
        /// <param name="x">Координата X центра круга</param>
        /// <param name="y">Координата Y центра круга</param>
        /// <param name="diameter">Диаметр круга</param>
        public void CreateCircle(ISketch sketch, int x, int y, int diameter)
        {
            IKompasDocument documentSketch = sketch.BeginEdit();
            IKompasDocument2D document2D = (IKompasDocument2D)documentSketch;
            IViewsAndLayersManager viewsAndLayersManager = document2D.ViewsAndLayersManager;
            IView view = viewsAndLayersManager.Views.ActiveView;
            IDrawingContainer drawingContainer = (IDrawingContainer)view;

            ICircle circle = drawingContainer.Circles.Add();
            circle.Style = (int)Kompas6Constants.ksCurveStyleEnum.ksCSNormal;
            circle.Xc = x;
            circle.Yc = y;
            circle.Radius = diameter / 2;
            circle.Update();
            sketch.EndEdit();
        }

        /// <summary>
        /// Метод для выдавливания эскиза
        /// </summary>
        /// <param name="sketch">Эскиз, который будет выдавлен</param>
        /// <param name="depth">Глубина экструзии</param>
        /// <param name="name">Имя экструзии</param>
        /// <param name="draftOutward">Флаг, указывающий направление экструзии</param>
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