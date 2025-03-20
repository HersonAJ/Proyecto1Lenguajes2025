namespace Analizadores
{

    public enum Estado
    {
        Q0, //estado inicial
        Q1,
        Q2,
        Q3,
        Q4,
        Q5,
        Q6,
        Q7,
        Q8,
        Q9,
        QF, //estado final o de aceptacion 

        QF_A, //estado de aceptacion para asignacion

        QF_R //estado de aceptacion para racional
    }
}