import { BookOpen, BookMarked, Users, AlertTriangle, Clock } from 'lucide-react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';

const metrics = [
  {
    title: 'Total Libros',
    value: '1.240',
    icon: BookOpen,
    description: '+12 este mes',
  },
  {
    title: 'Préstamos Activos',
    value: '85',
    icon: BookMarked,
    description: '32 por vencer hoy',
  },
  {
    title: 'Socios Registrados',
    value: '320',
    icon: Users,
    description: '+5 este mes',
  },
  {
    title: 'Libros Vencidos',
    value: '4',
    icon: AlertTriangle,
    description: 'Requieren atención',
    alert: true,
  },
];

const recentLoans = [
  { book: 'Cien Años de Soledad', member: 'María López', date: '2026-06-10' },
  { book: 'El Principito', member: 'Carlos Ruiz', date: '2026-06-09' },
  { book: 'Don Quijote de la Mancha', member: 'Ana García', date: '2026-06-08' },
];

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Dashboard</h1>
        <p className="text-sm text-muted-foreground">
          Bienvenido al panel de gestión de la biblioteca.
        </p>
      </div>

      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
        {metrics.map((metric) => {
          const Icon = metric.icon;
          return (
            <Card key={metric.title}>
              <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="text-sm font-medium text-muted-foreground">
                  {metric.title}
                </CardTitle>
                <Icon
                  className={`h-4 w-4 ${
                    metric.alert ? 'text-destructive' : 'text-muted-foreground'
                  }`}
                />
              </CardHeader>
              <CardContent>
                <div
                  className={`text-2xl font-bold ${
                    metric.alert ? 'text-destructive' : ''
                  }`}
                >
                  {metric.value}
                </div>
                <p className="text-xs text-muted-foreground">{metric.description}</p>
              </CardContent>
            </Card>
          );
        })}
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2 text-base">
            <Clock className="h-4 w-4" />
            Actividad Reciente
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b text-left">
                  <th className="pb-3 font-medium text-muted-foreground">Libro</th>
                  <th className="pb-3 font-medium text-muted-foreground">Socio</th>
                  <th className="pb-3 font-medium text-muted-foreground">Fecha</th>
                </tr>
              </thead>
              <tbody>
                {recentLoans.map((loan) => (
                  <tr key={`${loan.book}-${loan.member}`} className="border-b last:border-0">
                    <td className="py-3">{loan.book}</td>
                    <td className="py-3">{loan.member}</td>
                    <td className="py-3 text-muted-foreground">
                      {new Date(loan.date).toLocaleDateString('es-CO', {
                        year: 'numeric',
                        month: 'short',
                        day: 'numeric',
                      })}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}