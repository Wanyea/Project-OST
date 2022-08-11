using UnityEngine;

namespace PathCreation.Examples
{
    [ExecuteInEditMode]
    public abstract class PathSceneTool : MonoBehaviour
    {
        public event System.Action onDestroyed;
        public PathCreator pathCreator;
        public bool autoUpdate = true;

        private BezierPath sceneBezierPath;
        private PathCreator bezierCurve;

        void Start() 
        {
            GameObject sceneController = GameObject.Find("SceneController");
            BezierPathGeneration bezierPathGenerationScript = sceneController.GetComponent<BezierPathGeneration>();
            sceneBezierPath = bezierPathGenerationScript.bezierPath;

            VertexPath newVertexPath = new VertexPath(sceneBezierPath);

        
        
        }

        protected VertexPath path {
            get {
                return pathCreator.path;
            }
        }

        public void TriggerUpdate() {
            PathUpdated();
        }


        protected virtual void OnDestroy() {
            if (onDestroyed != null) {
                onDestroyed();
            }
        }

        protected abstract void PathUpdated();
    }
}
