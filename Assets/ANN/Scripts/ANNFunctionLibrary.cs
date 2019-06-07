using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace ANN
{
    public static class ANNFunctionLibrary
    {
        public static void AdjustWeights(ANN i_ANN, float[] i_DesiredOutputs)
        {
            if (i_ANN == null || i_DesiredOutputs == null || i_DesiredOutputs.Length != i_ANN.ANNOutputCount)
                return;

            // Init error gradients map (layer x neuron).

            List<List<float>> layerErrorGradients = new List<List<float>>();
            CommonFunctionLibrary.InitListNewElements<List<float>>(layerErrorGradients, i_ANN.layerCount);

            // Iterate all layers, starting from the output one: this is a backward propagation.

            for (int layerIndex = i_ANN.layerCount - 1; layerIndex >= 0; --layerIndex)
            {
                // Get layer and its error gradients entry.

                Layer layer = i_ANN.GetLayer(layerIndex);
                bool isLastLayer = (layerIndex == i_ANN.layerCount - 1);
                Layer nextLayer = (isLastLayer) ? null : i_ANN.GetLayer(layerIndex + 1);

                List<float> neuronErrorGradients = layerErrorGradients[layerIndex];
                List<float> nextLayerNeuronErrorGradients = (isLastLayer) ? null : layerErrorGradients[layerIndex + 1];

                // Iterate neuron.

                for (int neuronIndex = 0; neuronIndex < layer.neuronCount; ++neuronIndex)
                {
                    Neuron neuron = layer.GetNeuron(neuronIndex);
                    NeuronExecution lastNeuronExecution = neuron.lastNeuronExecution;

                    // Compute current error gradient.

                    float errorGradient = 0f;

                    if (isLastLayer)
                    {
                        // If this is the last layer just use iDesired - iActual.

                        float error = i_DesiredOutputs[neuronIndex] - lastNeuronExecution.output;
                        errorGradient = lastNeuronExecution.output * (1f - lastNeuronExecution.output) * error; // This is called DeltaRule (https://en.wikipedia.org/wiki/Delta_rule)
                    }
                    else
                    {
                        // If this is not the final layer, use a weighted error gradient baed on next layer error gradients.

                        errorGradient = lastNeuronExecution.output * (1f - lastNeuronExecution.output);

                        float nextLayerErrorGradientSum = 0f;
                        for (int nextLayerNeuronIndex = 0; nextLayerNeuronIndex < nextLayer.neuronCount; ++nextLayerNeuronIndex)
                        {
                            Neuron nextLayerNeuron = nextLayer.GetNeuron(nextLayerNeuronIndex);
                            float nextLayerErrorGradient = nextLayerNeuronErrorGradients[nextLayerNeuronIndex];
                            nextLayerErrorGradientSum += nextLayerErrorGradient * nextLayerNeuron.GetWeight(neuronIndex);
                        }

                        errorGradient *= nextLayerErrorGradientSum;
                    }

                    neuronErrorGradients.Add(errorGradient);

                    // Iterate over weights and adjust them.

                    for (int weightIndex = 0; weightIndex < neuron.inputCount; ++weightIndex)
                    {
                        float weight = neuron.GetWeight(weightIndex);

                        if (isLastLayer)
                        {
                            // If this is the last layer, act as do on a simple perceptron.

                            float error = i_DesiredOutputs[neuronIndex] - lastNeuronExecution.output;
                            weight = weight + i_ANN.alpha * lastNeuronExecution.GetInput(weightIndex) * error;
                        }
                        else
                        {
                            // If this is not the final layer, use error gradient as error.

                            weight = weight + i_ANN.alpha * lastNeuronExecution.GetInput(weightIndex) * errorGradient;
                        }

                        neuron.SetWeight(weightIndex, weight);
                    }

                    // Adjust bias as usual (keeping the learning rate).

                    neuron.bias = neuron.bias + i_ANN.alpha * errorGradient;
                }
            }
        }
    }
}