import { useState } from 'react';
import { BookOpen, Pencil, Shield, LockKeyhole, Plus, Search, ChevronLeft, ChevronRight } from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { useRoles, useToggleActivo, useUsuarios } from '@/hooks/useUsuarios';
import type { UsuarioDto, UsuariosFiltros } from '@/types/usuario.types';
import UsuarioFormModal from './UsuarioFormModal';
import AsignarRolModal from './AsignarRolModal';

export default function GestionUsuariosPage() {
  const { usuario } = useAuth();

  // Filtros locales
  const [nombre, setNombre] = useState('');
  const [email, setEmail] = useState('');
  const [rolId, setRolId] = useState('');
  const [activo, setActivo] = useState('');

  // Filtros que se envían al backend
  const [filtros, setFiltros] = useState<UsuariosFiltros>({
    pagina: 1,
    tamanoPagina: 15,
  });

  const { data, isLoading, isError } = useUsuarios(filtros);
  const { data: roles } = useRoles();
  const toggleActivo = useToggleActivo();

  // Modales
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedUsuario, setSelectedUsuario] = useState<UsuarioDto | null>(null);
  const [rolModalOpen, setRolModalOpen] = useState(false);
  const [usuarioParaRol, setUsuarioParaRol] = useState<UsuarioDto | null>(null);

  // Verificar permisos
  if (usuario?.rol !== 'Administrador') {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-sm text-muted-foreground">No tenés permisos para acceder a esta página.</p>
      </div>
    );
  }

  function handleBuscar() {
    setFiltros({
      nombre: nombre || undefined,
      email: email || undefined,
      rolId: rolId || undefined,
      activo: activo ? activo === 'true' : undefined,
      pagina: 1,
      tamanoPagina: filtros.tamanoPagina,
    });
  }

  function handlePagina(pagina: number) {
    setFiltros((prev) => ({ ...prev, pagina }));
  }

  function handleNuevoUsuario() {
    setSelectedUsuario(null);
    setModalOpen(true);
  }

  function handleEditarUsuario(usuario: UsuarioDto) {
    setSelectedUsuario(usuario);
    setModalOpen(true);
  }

  function handleAsignarRol(usuario: UsuarioDto) {
    setUsuarioParaRol(usuario);
    setRolModalOpen(true);
  }

  async function handleToggleActivo(usuario: UsuarioDto) {
    await toggleActivo.mutateAsync({
      id: usuario.id,
      activar: !usuario.activo,
    });
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-black">Gestión de Usuarios</h1>
          <p className="text-sm text-muted-foreground">Administrá los usuarios registrados en el sistema.</p>
        </div>
        <button
          onClick={handleNuevoUsuario}
          className="inline-flex items-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90"
        >
          <Plus className="h-4 w-4" />
          Nuevo usuario
        </button>
      </div>

      {/* Filtros */}
      <div className="rounded-lg border bg-card p-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-6">
          <div>
            <label className="text-sm font-medium">Nombre</label>
            <input
              type="text"
              value={nombre}
              onChange={(e) => setNombre(e.target.value)}
              placeholder="Buscar por nombre..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="text-sm font-medium">Email</label>
            <input
              type="text"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Buscar por email..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="text-sm font-medium">Rol</label>
            <select
              value={rolId}
              onChange={(e) => setRolId(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              <option value="">Todos</option>
              {roles?.map((rol) => (
                <option key={rol.id} value={rol.id}>{rol.nombre}</option>
              ))}
            </select>
          </div>
          <div>
            <label className="text-sm font-medium">Estado</label>
            <select
              value={activo}
              onChange={(e) => setActivo(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              <option value="">Todos</option>
              <option value="true">Activo</option>
              <option value="false">Inactivo</option>
            </select>
          </div>
          <div className="flex items-end">
            <button
              onClick={handleBuscar}
              className="w-full rounded-md bg-secondary px-4 py-2 text-sm font-medium text-secondary-foreground hover:bg-secondary/80"
            >
              <Search className="mr-1 inline h-4 w-4" />
              Buscar
            </button>
          </div>
        </div>
      </div>

      {/* Tabla */}
      {isLoading && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-muted-foreground">Cargando usuarios...</p>
        </div>
      )}

      {isError && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-destructive">Ocurrió un error al cargar los usuarios.</p>
        </div>
      )}

      {data && data.items.length === 0 && (
        <div className="flex flex-col items-center py-12 text-center">
          <BookOpen className="h-12 w-12 text-muted-foreground" />
          <p className="mt-4 text-sm text-muted-foreground">No se encontraron usuarios con los filtros seleccionados.</p>
        </div>
      )}

      {data && data.items.length > 0 && (
        <div className="overflow-x-auto rounded-lg border">
          <table className="w-full text-sm">
            <thead className="bg-muted/50">
              <tr>
                <th className="px-4 py-3 text-left font-medium">Nombre completo</th>
                <th className="px-4 py-3 text-left font-medium">Email</th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">Documento</th>
                <th className="px-4 py-3 text-left font-medium hidden sm:table-cell">Rol</th>
                <th className="px-4 py-3 text-center font-medium">Estado</th>
                <th className="px-4 py-3 text-right font-medium">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {data.items.map((u) => (
                <tr key={u.id} className="hover:bg-muted/30">
                  <td className="px-4 py-3 font-medium">{u.nombre} {u.apellido}</td>
                  <td className="px-4 py-3">{u.email}</td>
                  <td className="px-4 py-3 hidden md:table-cell">{u.numeroDocumento}</td>
                  <td className="px-4 py-3 hidden sm:table-cell">{u.nombreRol}</td>
                  <td className="px-4 py-3 text-center">
                    {u.activo ? (
                      <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">Activo</span>
                    ) : (
                      <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">Inactivo</span>
                    )}
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex items-center justify-end gap-1">
                      <button
                        onClick={() => handleEditarUsuario(u)}
                        className="rounded-md p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                        title="Editar"
                      >
                        <Pencil className="h-4 w-4" />
                      </button>
                      <button
                        onClick={() => handleAsignarRol(u)}
                        className="rounded-md p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                        title="Cambiar rol"
                      >
                        <Shield className="h-4 w-4" />
                      </button>
                      <button
                        onClick={() => handleToggleActivo(u)}
                        disabled={toggleActivo.isPending}
                        className="rounded-md p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                        title={u.activo ? 'Desactivar' : 'Activar'}
                      >
                        <LockKeyhole className="h-4 w-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Paginador */}
      {data && data.totalPaginas > 0 && (
        <div className="flex items-center justify-between">
          <p className="text-sm text-muted-foreground">
            Página {data.paginaActual} de {data.totalPaginas} — {data.totalRegistros} registros
          </p>
          <div className="flex gap-2">
            <button
              onClick={() => handlePagina(data.paginaActual - 1)}
              disabled={!data.tienePaginaAnterior}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              <ChevronLeft className="inline h-4 w-4" /> Anterior
            </button>
            <button
              onClick={() => handlePagina(data.paginaActual + 1)}
              disabled={!data.tienePaginaSiguiente}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Siguiente <ChevronRight className="inline h-4 w-4" />
            </button>
          </div>
        </div>
      )}

      {/* Modal crear/editar */}
      <UsuarioFormModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        usuario={selectedUsuario}
      />

      {/* Modal asignar rol */}
      <AsignarRolModal
        open={rolModalOpen}
        onClose={() => setRolModalOpen(false)}
        usuario={usuarioParaRol}
      />
    </div>
  );
}