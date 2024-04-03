using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ViewChange : MonoBehaviour
{
    [Header("正交大小")]
    public float orthoSize = 10;
    public static ViewChange Instance = null;

    /// <summary>
    /// 相机透视改变是否触发(调用只需把此值改为true)
    /// </summary>
    public bool ChangeProjection = false;
    public bool _changing = false;
    public float ProjectionChangeTime = 2f;
    private float _currentT = 0.0f;

    public bool isOrthographic = false;

    public Quaternion orthographicRotation = Quaternion.LookRotation(new Vector3(-1, -1, 1), Vector3.up);



    //跟随
    public GameObject target;
    public float persDistance = 6;
    public float orthoDistance = 100;
    public float currentDistance = 6;

    float horizontal = 0;
    float vertical = 0;
    Vector3 lastMousePos;
    Vector3 mouseDelta;

    // Start is called before the first frame update
    void Start()
    {
        lastMousePos = Input.mousePosition;
        mouseDelta = Vector2.zero;
    }

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    { ///检测，避免变换过程中发生混乱
		if (_changing)
        {
            ChangeProjection = false;
        }
        else if (ChangeProjection)
        {
            _changing = true;
            _currentT = 0.0f;

            //var cameraFollow = GetComponent<CameraFollow>();
            //Vector3 pos = cameraFollow.target.transform.position - cameraFollow.distance * Camera.main.transform.forward;
            //Camera.main.transform.position = pos;
        }

        if (_changing && isOrthographic == false || _changing == false && isOrthographic == true)
        {
            LerpRotation();
        }


        if (Input.GetKey(KeyCode.LeftAlt))
        {
            mouseDelta = Vector3.zero;
            lastMousePos = Input.mousePosition;
        }
        else
        {
            mouseDelta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;
        }
    }
    /// <summary>
    /// Unity3D中Update和Lateupdate的区别。Lateupdate和Update每一祯都被执行，但是执行顺序不一样，先执行Updatee然后执行lateUpdate。
    ///如果你有两个脚本JS1、JS2，两个脚本中都有Update()函数, 在JS1中有 lateUpdate ,JS2中没有。那么 lateUpdate 函数会等待JS1、JS2两个脚本的Update()函数 都执行完后才执行。
    /// </summary>
    private void LateUpdate()
    {
        if (_changing == false && isOrthographic == false)
        {
            ChangeAngle();
        }
        else
        {
            vertical = Camera.main.transform.rotation.eulerAngles.x;
            horizontal = Camera.main.transform.rotation.eulerAngles.y;
        }

        if (target != null)
        {

            Vector3 pos = target.transform.position - currentDistance * Camera.main.transform.forward;
            Camera.main.transform.position = pos;
        }



        if (!_changing)
        {
            return;
        }
        //将当前的 是否正视图值 赋值给currentlyOrthographic变量
        bool currentlyOrthographic = Camera.main.orthographic;
        //定义变量存放当前摄像机的透视和正视矩阵信息；
        Matrix4x4 orthoMat, persMat;
        Matrix4x4 temp = Camera.main.projectionMatrix;

        Camera.main.orthographic = true;
        Camera.main.ResetProjectionMatrix();
        orthoMat = GetOrtho(orthoSize);

        Camera.main.orthographic = false;
        Camera.main.ResetProjectionMatrix();
        persMat = Camera.main.projectionMatrix;


        Camera.main.projectionMatrix = temp;

        currentlyOrthographic = isOrthographic;
        _currentT += (Time.deltaTime / ProjectionChangeTime);


        if (_currentT < 1.0f)
        {
            if (currentlyOrthographic)
            {
                if (_currentT < 0.5f)
                {
                    Camera.main.projectionMatrix = MatrixLerp(orthoMat, persMat, _currentT * 2 * _currentT * 2);
                }
                else
                {
                    currentDistance = Mathf.Lerp(orthoDistance, persDistance, (_currentT - 0.5f) * 2 * (_currentT - 0.5f) * 2);
                }
            }
            else
            {
                if (_currentT < 0.5f)
                {
                    currentDistance = Mathf.Lerp(persDistance, orthoDistance, _currentT * 2 * _currentT * 2);
                }
                else
                {
                    Camera.main.projectionMatrix = MatrixLerp(persMat, orthoMat, Mathf.Sqrt((_currentT - 0.5f) * 2));
                }
            }
        }
        else
        {
            if (currentlyOrthographic)
            {
                Camera.main.projectionMatrix = MatrixLerp(orthoMat, persMat, 1);
                currentDistance = Mathf.Lerp(orthoDistance, persDistance, 1);
            }
            else
            {
                Camera.main.projectionMatrix = MatrixLerp(persMat, orthoMat, 1);
                currentDistance = Mathf.Lerp(persDistance, orthoDistance, 1);
            }
            _changing = false;
            isOrthographic = !isOrthographic;
            //Camera.main.orthographic = !currentlyOrthographic;
            //Camera.main.ResetProjectionMatrix();
        }








    }

    Matrix4x4 GetOrtho(float size)
    {
        float aspect = Camera.main.aspect;
        float far = Camera.main.farClipPlane;
        float near = Camera.main.nearClipPlane;
        Matrix4x4 newMatrix = new();
        newMatrix.SetRow(0, new Vector4(1 / aspect / size, 0, 0, 0));
        newMatrix.SetRow(1, new Vector4(0, 1 / size, 0, 0));
        newMatrix.SetRow(2, new Vector4(0, 0, -2 / (far - near), -(far + near) / (far - near)));
        newMatrix.SetRow(3, new Vector4(0, 0, 0, 1));
        return newMatrix;
    }

    private Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float t)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        Matrix4x4 newMatrix = new Matrix4x4();
        newMatrix.SetRow(0, Vector4.Lerp(from.GetRow(0), to.GetRow(0), t));
        newMatrix.SetRow(1, Vector4.Lerp(from.GetRow(1), to.GetRow(1), t));
        newMatrix.SetRow(2, Vector4.Lerp(from.GetRow(2), to.GetRow(2), t));
        newMatrix.SetRow(3, Vector4.Lerp(from.GetRow(3), to.GetRow(3), t));
        return newMatrix;
    }

    void OnGUI()
    {
        GUILayout.Label("New Camera.main.projectionMatrix:\n" + Camera.main.projectionMatrix.ToString());

        //将当前的 是否正视图值 赋值给currentlyOrthographic变量
        bool currentlyOrthographic = Camera.main.orthographic;
        //定义变量存放当前摄像机的透视和正视矩阵信息；
        Matrix4x4 orthoMat, persMat;
        Matrix4x4 temp = Camera.main.projectionMatrix;

        Camera.main.orthographic = true;
        Camera.main.ResetProjectionMatrix();
        orthoMat = GetOrtho(orthoSize);

        Camera.main.orthographic = false;
        Camera.main.ResetProjectionMatrix();
        persMat = Camera.main.projectionMatrix;


        Camera.main.projectionMatrix = temp;

        GUILayout.Label("orthoMatrix:\n" + orthoMat.ToString());
        GUILayout.Label("persMatrix:\n" + persMat.ToString());


        if (GUILayout.Button("更改CameraProjection"))
        {
            ChangeProjection = true;
        }
    }


    void LerpRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, orthographicRotation, Mathf.Clamp(_currentT, 0.0f, 1.0f));
    }

    void ChangeAngle()
    {
        horizontal += mouseDelta.x * 0.2f;
        horizontal = (horizontal + 360) % 360;
        vertical -= mouseDelta.y * 0.2f;
        vertical = Mathf.Clamp(vertical, -20, 60);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, horizontal, 0)) * Quaternion.Euler(new Vector3(vertical, 0, 0));
        Camera.main.transform.rotation = rotation;
    }
}
