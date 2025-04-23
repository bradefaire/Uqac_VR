import os
import pandas as pd
from scipy.stats import friedmanchisquare, wilcoxon
import seaborn as sns
import matplotlib.pyplot as plt

# === Chargement des données === #
files = []
index = 1
for file in os.listdir():
    if os.path.splitext(file)[1] == ".csv":
        df_temp = pd.read_csv(file)
        df_temp = df_temp[df_temp['reaction time'] != -1]  # Exclure les temps invalides
        df_temp = df_temp[df_temp['reaction time'] < 10]  # Exclure les temps trop élevés
        df_temp['participant'] = index
        index += 1
        files.append(df_temp)

# Concaténation de tous les participants dans le même tableau dataframe
df = pd.concat(files, ignore_index=True)

# === Tests statistiques === #

distances = [5, 15, 25]

for d in distances:
    print(f"\n--- Analyse pour distance = {d} m ---")
    # Filtrer les données pour cette distance
    df_d = df[df['target distance'] == d]
    
    # Réorganiser en tableau : lignes = participants, colonnes = positions
    table = df_d.pivot_table(index='participant', columns='position', values='reaction time', aggfunc='mean')
    table = table.drop(columns=['poche'], errors='ignore')  # Supprimer 'poche' si elle existe
    table = table.dropna()

    # Vérifier si chaque participant a testé toutes les positions
    if table.shape[1] == 2:  # Si seulement 2 positions restent on peut faire un test de Wilcoxon
        stat, p = wilcoxon(table.iloc[:, 0], table.iloc[:, 1])
        print(f"Statistique de Wilcoxon : {stat:.2f}")
        print(f"p-value : {p:.4f}")
        if p < 0.05:
            print("⇒ Différence significative entre les positions !")
        else:
            print("⇒ Aucune différence significative entre les positions.")
        
        # Meilleure position (temps moyen minimal)
        mean_reaction_times = table.mean()
        best_position = mean_reaction_times.idxmin()
        print(f"Meilleure position : {best_position} (temps moyen : {mean_reaction_times[best_position]:.2f} s)")

    elif table.shape[1] > 2:  # Si plus de 2 positions restent on peut faire un test de Friedman
        stat, p = friedmanchisquare(*[table[col] for col in table.columns])
        print(f"Statistique de Friedman : {stat:.2f}")
        print(f"p-value : {p:.4f}")
        if p < 0.05:
            print("⇒ Différence significative entre les positions !")
        else:
            print("⇒ Aucune différence significative entre les positions.")
        
        # Meilleure position (temps moyen minimal)
        mean_reaction_times = table.mean()
        best_position = mean_reaction_times.idxmin()
        print(f"Meilleure position : {best_position} (temps moyen : {mean_reaction_times[best_position]:.2f} s)")

    else:
        print("Pas assez de positions pour effectuer un test statistique.")

    # Calcul des moyennes par position pour chaque distance
    mean_by_position = df_d.groupby('position')['reaction time'].mean()
    mean_df = pd.DataFrame({'Position': mean_by_position.index, 'Moyenne': mean_by_position.values, 'Distance': d})

    # Calcul des écarts-types par position pour chaque distance
    std_by_position = df_d.groupby('position')['reaction time'].std()
    std_df = pd.DataFrame({'Position': std_by_position.index, 'Écart-type': std_by_position.values, 'Distance': d})

    # Concaténer les résultats pour toutes les distances
    if 'mean_results' not in locals():
        mean_results = mean_df
        std_results = std_df
    else:
        mean_results = pd.concat([mean_results, mean_df], ignore_index=True)
        std_results = pd.concat([std_results, std_df], ignore_index=True)

# Identifier les positions testées par au moins la moitié des participants pour chaque distance
threshold = len(df['participant'].unique()) / 2
valid_positions = df.groupby(['position'])['participant'].nunique().reset_index(name='Count')
valid_positions = valid_positions[valid_positions['Count'] >= threshold]

# Visualisation des écarts-types
plt.figure()
for pos in std_results['Position'].unique():
    pos_data = std_results[std_results['Position'] == pos]
    if pos in valid_positions['position'].values:
        custom_linestyle = '-'
        custom_linewidth = 1.5
        custom_markersize = 5
    else:
        custom_linestyle = '--'
        custom_linewidth = 1
        custom_markersize = 3
    plt.plot(pos_data['Distance'], pos_data['Écart-type'], marker='o', label=pos, linestyle=custom_linestyle, linewidth=custom_linewidth, markersize=custom_markersize)
plt.title("Écart-type des temps de réaction")
plt.ylabel("Écart-type des temps de réaction (s)")
plt.xlabel("Distance (m)")
plt.legend(title="Position")
plt.grid(axis='both', alpha=0.7)
plt.xticks(ticks=range(5, 26, 5))
plt.show()

# Visualisation des moyennes
plt.figure()
i=0
for pos in mean_results['Position'].unique():
    pos_data = mean_results[mean_results['Position'] == pos]
    i+=1
    if pos in valid_positions['position'].values:
        custom_linestyle = '-'
        custom_linewidth = 1.5
        custom_markersize = 5
    else:
        custom_linestyle = '--'
        custom_linewidth = 1
        custom_markersize = 3
    plt.plot(pos_data['Distance'], pos_data['Moyenne'], marker='o', label=pos, linestyle=custom_linestyle, linewidth=custom_linewidth, markersize=custom_markersize)
plt.title("Moyenne des temps de réaction")
plt.ylabel("Moyenne des temps de réaction (s)")
plt.xlabel("Distance (m)")
plt.legend(title="Position")
plt.grid(axis='both', alpha=0.7)
plt.xticks(ticks=range(5, 26, 5))
plt.show()