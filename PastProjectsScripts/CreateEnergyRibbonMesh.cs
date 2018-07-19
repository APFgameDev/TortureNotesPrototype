using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreateEnergyRibbonMesh : MonoBehaviour
{
    [SerializeField]
    Transform m_startPos;
    [SerializeField]
    Transform m_endPos;

    [SerializeField]
    uint m_numSegments;

    [SerializeField]
    float m_endThickness = 1;

    [SerializeField]
    float m_startThickness = 1;
    [SerializeField]
    Vector3 m_debugCubeSize = Vector3.one;

    [SerializeField]
    float m_waveMag = 0.1f;

    public float m_jiggleSpeed;
    [SerializeField]
    float m_randMaxUpdateTime = 2;
    [SerializeField]
    float m_randMinUpdateTime = 0.1f;

    [SerializeField]
    float m_waveFreq = 2;

    [SerializeField]
    float m_randomMag;

    [SerializeField]
    float m_randDepthMag;

    [SerializeField]
    float m_waveSpeed =2;

    [SerializeField]
    uint m_RandomSamples = 2;

    [SerializeField]
    float m_waveOffset = 2;

    [SerializeField]
    float m_minThicknessMult = 0.1f;

    [SerializeField]
    float m_maxThicknessMult = 1.0f;

    [SerializeField]
    float m_heightGain = 5;

    [SerializeField]
    float m_minHeight = 5;

    Vector3[] m_verts;
    float[] m_offsets;
    float[] m_thicknessVaration;
    float[] m_randRot;
    float[] m_randDepth;

    float[] randsToLerpTo;
    float[] randHeightsToLerpTo;
    float[] randRotToLerpTo;
    float[] randDepthToLerpTo;


    [SerializeField]
    float m_thincknessSpeed = 2;

    float timeTillChange = 0;

    Mesh m_mesh;
    float time = 0;

    //per row
    uint m_numVerts;
    uint m_numTris;

	// Use this for initialization
	void Start ()
    {
        m_mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = m_mesh;

        CreateMesh();
       // StartCoroutine(ChangeOffsets());
    }

    void CreateMesh()
    {
        m_numVerts = 2 + m_numSegments;

        m_verts = new Vector3[m_numVerts * 2];
        m_offsets = new float[m_RandomSamples];
        m_thicknessVaration = new float[m_RandomSamples];
        m_randRot = new float[m_RandomSamples];
        m_randDepth = new float[m_RandomSamples];

        randsToLerpTo = new float[m_RandomSamples];
        randHeightsToLerpTo = new float[m_RandomSamples];
        randRotToLerpTo = new float[m_RandomSamples];
        randDepthToLerpTo = new float[m_RandomSamples];

        Vector3 startPosLocal = transform.InverseTransformPoint(m_startPos.position);

        Vector3 endPosLocal = transform.InverseTransformPoint(m_endPos.position);

        Vector3 thickness = Vector3.up * m_endThickness;

        m_numTris = m_numVerts * 2 - 2;
        m_numTris *= 3;

        int[] m_tris = new int[m_numTris];

        Vector2[] uvs = new Vector2[m_numVerts * 2];

        for (int i = 0; i < m_numVerts; i++)
        {
            float lerpPercentage = i / (float)(m_numVerts - 1);


            m_verts[i] = Vector3.Lerp(startPosLocal, endPosLocal, lerpPercentage) + thickness;
            m_verts[i + m_numVerts] = Vector3.Lerp(startPosLocal, endPosLocal, lerpPercentage) - thickness;

            uvs[i] = new Vector2(lerpPercentage, 0);
            uvs[i + m_numVerts] = new Vector2(lerpPercentage, 1);
        }

        uint halfTris = m_numVerts - 1;

        int vertI = 0;

        for (int i = 0; i < m_numTris; i += 6)
        {
            m_tris[i] = vertI;
            m_tris[i + 1] = vertI + 1;
            m_tris[i + 2] = vertI + (int)m_numVerts;

            m_tris[i + 3] = vertI + 1;
            m_tris[i + 4] = vertI + (int)m_numVerts + 1;
            m_tris[i + 5] = vertI + (int)m_numVerts;
            vertI++;
        }


        m_mesh.vertices = m_verts;
        m_mesh.triangles = m_tris;

        m_mesh.uv = uvs;

        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();

    }

    void Update()
    {
        float midValue = (m_numVerts + 1) * 0.5f;
        //Greater When In Middle
        float magMult = 0;

        Vector3 startPosLocal = transform.InverseTransformPoint(m_startPos.position);
        Vector3 endPosLocal = transform.InverseTransformPoint(m_endPos.position);
        Vector3 thickness;
        Vector3 randVal;
     
        Vector3 offSet;

        float deltaRands = (float)m_RandomSamples / m_numVerts;

        Vector3 RelativeUp = transform.rotation * Vector3.up;

        float waveMove = Time.timeSinceLevelLoad * Time.timeScale * m_waveSpeed;

        for (int i = 0; i < m_numVerts; i++)
        {
            float randLerp = deltaRands * i;
            int randIndex = (int)randLerp;
            randLerp -= randIndex;

            float randValFloat = 0;
            float randRotVal = 0;
            float randThickness = 0;
            float randDepth = 0;

            if (randIndex + 1 < m_RandomSamples)
            {
                randRotVal = Mathf.Lerp(m_randRot[randIndex], m_randRot[randIndex + 1], randLerp);
                randValFloat = Mathf.Lerp(m_offsets[randIndex], m_offsets[randIndex + 1], randLerp);
                randThickness = Mathf.Lerp(m_thicknessVaration[randIndex], m_thicknessVaration[randIndex + 1], randLerp);
                randDepth = Mathf.Lerp(m_randDepth[randIndex], m_randDepth[randIndex + 1], randLerp);
            }
            else
            {
                randValFloat = m_offsets[randIndex];
                randRotVal = m_randRot[randIndex];
                randThickness = m_thicknessVaration[randIndex];
                randDepth = m_randDepth[randIndex];
            }

            magMult = Mathf.Abs(midValue - (i));
            magMult =  1 - (magMult / midValue);

            float lerpPercentage = i / (float)(m_numVerts - 1);

            randVal = RelativeUp * randValFloat * magMult;
            randThickness *= magMult;

            Vector3 randUp = Quaternion.AngleAxis(randRotVal, (m_startPos.position - m_endPos.position).normalized) * RelativeUp;

            float lerpedThickness = Mathf.Lerp(m_startThickness, m_endThickness, lerpPercentage);

            Vector3 normal = Vector3.Cross(randUp, (m_startPos.position - m_endPos.position).normalized);

            thickness = randUp * lerpedThickness + randUp * randThickness * lerpedThickness;

            offSet = RelativeUp * Mathf.Sin(waveMove + lerpPercentage * m_waveFreq + m_waveOffset) * m_waveMag * magMult + RelativeUp * MathUtils.Sinerp(0f,1f, magMult) * m_heightGain;

            m_verts[i] = Vector3.Lerp(startPosLocal, endPosLocal, lerpPercentage) + thickness + randVal;
            m_verts[i + m_numVerts] = Vector3.Lerp(startPosLocal, endPosLocal, lerpPercentage) - thickness + randVal;

            Vector3 depthOffset = randDepth * normal * magMult;

            m_verts[i] += offSet + depthOffset;
            m_verts[i + m_numVerts] += offSet + depthOffset;

        }
     
        m_mesh.vertices = m_verts;

        m_mesh.RecalculateBounds();
        m_mesh.RecalculateNormals();
        m_mesh.RecalculateTangents();

        ChangeOffsets();
    }

    public void SetOffsets()
    {
        for (int i = 0; i < m_RandomSamples; i++)
        {
            m_offsets[i] = Random.Range(-m_randomMag, m_randomMag);
            m_thicknessVaration[i] = Random.Range(m_minThicknessMult, m_maxThicknessMult);
            randRotToLerpTo[i] = Random.Range(-90, 90);
            randDepthToLerpTo[i] = Random.Range(-m_randDepthMag, m_randDepthMag);
        }
    }

    void ChangeOffsets()
    {
        float deltaRands = (float)m_RandomSamples / m_numVerts;

        time += Time.deltaTime;

        if (time < timeTillChange)
        {
            for (int i = 0; i < m_RandomSamples; i++)
            {
                m_offsets[i] = MathUtils.Sinerp(m_offsets[i], randsToLerpTo[i], Time.deltaTime * m_jiggleSpeed);
                m_thicknessVaration[i] = MathUtils.Sinerp(m_thicknessVaration[i], randHeightsToLerpTo[i], Time.deltaTime * m_thincknessSpeed);
                m_randRot[i] = MathUtils.Sinerp(m_randRot[i], randRotToLerpTo[i], Time.deltaTime * m_jiggleSpeed);
                m_randDepth[i] = MathUtils.Sinerp(m_randDepth[i], randDepthToLerpTo[i], Time.deltaTime * m_jiggleSpeed);
            }
        }
        else
        {
            time = 0;
            timeTillChange = Random.Range(m_randMinUpdateTime, m_randMaxUpdateTime);
            for (int i = 0; i < m_RandomSamples; i++)
            {
                randsToLerpTo[i] = Random.Range(-m_randomMag, m_randomMag);
                randHeightsToLerpTo[i] = Random.Range(m_minThicknessMult, m_maxThicknessMult);
                randRotToLerpTo[i] = Random.Range(-90, 90);
                randDepthToLerpTo[i] = Random.Range(-m_randDepthMag, m_randDepthMag);
            }
        }
    }


}
