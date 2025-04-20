import os
import pandas as pd
from scipy.stats import friedmanchisquare

## Traitement ##

# Charger les données
files = []
index = 1
for file in os.listdir("Resultats"):
    if os.path.splitext(file)[1] == ".csv":
        df_temp = pd.read_csv(os.path.join("Resultats", file))
        df_temp = df_temp[df_temp['reaction time'] != -1]
        df_temp = df_temp[df_temp['reaction time'] < 10]
        df_temp['participant'] = index
        index += 1
        files.append(df_temp)
        #print(df_temp)

# Combine all individual tester data into a single DataFrame
df = pd.concat(files, ignore_index=True)

# Liste des distances testées
distances = [5,15,25]

for d in distances:
    print(f"\n--- Analyse pour distance = {d} m ---")
    # Filtrer les données pour cette distance
    df_d = df[df['target distance'] == d]
    
    # Réorganiser en tableau : lignes = participants, colonnes = positions
    table = df_d.pivot_table(index='participant', columns='position', values='reaction time', aggfunc='mean')
    print(table)

    # Vérifie que chaque participant a bien testé toutes les positions
    if table.isnull().any().any():
        print("Données manquantes pour cette distance, test non effectué.")
        continue

    # Appliquer le test de Friedman
    stat, p = friedmanchisquare(*[table[col] for col in table.columns])
    print(f"Statistique de Friedman : {stat:.2f}")
    print(f"p-value : {p:.4f}")

    if p < 0.05:
        print("⇒ Différence significative entre les positions !")
    else:
        print("⇒ Aucune différence significative entre les positions.")


## Visualisation ##

import seaborn as sns
import matplotlib.pyplot as plt

sns.set(style="whitegrid")

for d in distances:
    df_d = df[df['target distance'] == d]
    plt.figure()
    sns.boxplot(x='position', y='reaction time', data=df_d)
    plt.title(f"Temps de réaction par position (distance {d} m)")
    plt.ylabel("Temps de réaction (ms)")
    plt.xlabel("Position")
    plt.show()