using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ANNDrive : MonoBehaviour {

    ANN ann;
    public float visibleDistance = 200f;
    public int epchos = 1000;
    public float speedForce = 50f;
    public float rotationSpeed = 100f;

    bool trainingDone = false;
    float trainingProgress = 0;
    double sumSquaredErrors = 0;
    double lastSumSquaredErrors = 1;

    public float translation;
    public float rotation;

    public bool LoadFromFile = false; 

    void Start () {
        ann = new ANN(5, 2, 1, 10, 0.5);
        if(LoadFromFile)
        {
            LoadWeightsFromFile();
            trainingDone = true;
        }
        else
        StartCoroutine(LoadTrainingSet());
	}
    private void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 250, 30), "SSE " + lastSumSquaredErrors);
        GUI.Label(new Rect(25, 40, 250, 30), "Alpha " + ann.alpha);
        GUI.Label(new Rect(25, 55, 250, 30), "Trained " + trainingProgress);
    }
    private IEnumerator LoadTrainingSet()
    {
        string path = Application.dataPath + "/trainData.txt";
        string line;
        if(File.Exists(path))
        {
            int lineCount = File.ReadAllLines(path).Length;
            StreamReader dataFile = File.OpenText(path);
            List<double> calcOutputs = new List<double>();
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();

            for(int i = 0; i < epchos; i++)
            {
                sumSquaredErrors = 0;
                dataFile.BaseStream.Position = 0;
                string currentWeights = ann.PrintWeights();
                while( (line = dataFile.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    float thisError = 0;
                    if( System.Convert.ToDouble(data[5]) != 0 && System.Convert.ToDouble(data[6]) != 0)
                    {
                        inputs.Clear();
                        outputs.Clear();
                        inputs.Add(System.Convert.ToDouble(data[0]));
                        inputs.Add(System.Convert.ToDouble(data[1]));
                        inputs.Add(System.Convert.ToDouble(data[2]));
                        inputs.Add(System.Convert.ToDouble(data[3]));
                        inputs.Add(System.Convert.ToDouble(data[4]));

                        double o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[5]));
                        outputs.Add(o1);
                        double o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[6]));
                        outputs.Add(o2);

                        calcOutputs = ann.Train(inputs, outputs);
                        thisError = (Mathf.Pow((float)(outputs[0] - calcOutputs[0]), 2) * Mathf.Pow((float)(outputs[1] - calcOutputs[1]), 2))/ 2.0f;
                    }
                    sumSquaredErrors += thisError;
                }
                trainingProgress = (float)i / (float)epchos;
                sumSquaredErrors /= lineCount;
                if(lastSumSquaredErrors < sumSquaredErrors)
                {
                    ann.LoadWeights(currentWeights);
                    ann.alpha = Mathf.Clamp((float)ann.alpha - 0.001f, 0.01f, 0.9f);
                }
                else
                {
                    ann.alpha = Mathf.Clamp((float)ann.alpha + 0.001f, 0.01f, 0.9f);
                    lastSumSquaredErrors = sumSquaredErrors;
                }

                yield return null;
            }
        }
        trainingDone = true;
        SaveWeightsToFile();
    }
    void SaveWeightsToFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamWriter wf = File.CreateText(path);
        wf.WriteLine(ann.PrintWeights());
        wf.Close();
    }
    void LoadWeightsFromFile()
    {
        string path = Application.dataPath + "/weights.txt";
        StreamReader wf = File.OpenText(path);
        if(File.Exists(path))
        {
            string line = wf.ReadLine();
            ann.LoadWeights(line);
        }
    }

    float Map(float newFrom, float newTo, float origFrom, float origTo, float value)
    {
        if(value <= origFrom)
        {
            return newFrom;
        }
        else if(value >= origTo)
        {
            return newTo;
        }
        return (newTo - newFrom) * (value - origFrom) / (origTo - origFrom) * newFrom;
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }
    // Update is called once per frame
    void Update () {
		if(!trainingDone) { return; }

        List<double> calcOutputs = new List<double>();
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();

        RaycastHit hit;
        float fDist = 0, rDist = 0, lDist = 0,
                      r45Dist = 0, l45Dist = 0;
        if (Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
            fDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
            rDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
            lDist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * this.transform.right, out hit, visibleDistance))
        {
            r45Dist = 1 - Round(hit.distance / visibleDistance);
        }
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))
        {
            l45Dist = 1 - Round(hit.distance / visibleDistance);
        }

        inputs.Add(fDist);
        inputs.Add(rDist);
        inputs.Add(lDist);
        inputs.Add(r45Dist);
        inputs.Add(l45Dist);

        outputs.Add(0);
        outputs.Add(0);
        calcOutputs = ann.CalcOutput(inputs, outputs);
        float translationInput = Map(-1, 1, 0, 1, (float)calcOutputs[0]);
        float roatationInput = Map(-1, 1, 0, 1, (float)calcOutputs[1]);
        translation = translationInput * speedForce * Time.deltaTime;
        rotation = roatationInput * rotationSpeed * Time.deltaTime;
        this.transform.Translate(0, 0, translation);
        this.transform.Rotate(0, rotation, 0);
    }
}
/*
 List<string> trainData = new List<string>();
    StreamWriter dateFile;
    private void Start()
    {
        string path = Application.dataPath + "/trainData.txt";
        dateFile = File.CreateText(path);
        rb = GetComponent<Rigidbody>();
    }
    float Round(float x)
    {
        return (float)System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2.0f;
    }

 private void OnApplicationQuit()
    {
        foreach(string tData in trainData)
        {
            dateFile.WriteLine(tData);
        }
        dateFile.Close();
    }

RaycastHit hit;
float fDist = 0, rDist = 0, lDist = 0,
              r45Dist = 0, l45Dist = 0;
        if(Physics.Raycast(transform.position, this.transform.forward, out hit, visibleDistance))
        {
    fDist = 1 - Round(hit.distance / visibleDistance);
}
        if (Physics.Raycast(transform.position, this.transform.right, out hit, visibleDistance))
        {
    rDist = 1 - Round(hit.distance / visibleDistance);
}
        if (Physics.Raycast(transform.position, -this.transform.right, out hit, visibleDistance))
        {
    lDist = 1 - Round(hit.distance / visibleDistance);
}
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * this.transform.right, out hit, visibleDistance))
        {
    r45Dist = 1 - Round(hit.distance / visibleDistance);
}
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -this.transform.right, out hit, visibleDistance))
        {
    l45Dist = 1 - Round(hit.distance / visibleDistance);
}

        string data = fDist + "," + rDist + "," + lDist + "," + r45Dist + "," + l45Dist + "," +
                      Round(rawTranslation) + "," + Round(rawRotation);
        if(!trainData.Contains(data))
        {
    trainData.Add(data);
}
*/
