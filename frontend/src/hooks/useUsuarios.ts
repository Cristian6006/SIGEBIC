import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  asignarRol,
  cambiarPassword,
  createUsuario,
  getPerfil,
  getRoles,
  getUsuarioById,
  getUsuarios,
  toggleActivo,
  updateUsuario,
} from '@/api/usuarios.api';
import type {
  CambiarPasswordRequest,
  CreateUsuarioRequest,
  UpdateUsuarioRequest,
  UsuariosFiltros,
} from '@/types/usuario.types';

export function useUsuarios(filtros: UsuariosFiltros) {
  return useQuery({
    queryKey: ['usuarios', filtros],
    queryFn: () => getUsuarios(filtros),
  });
}

export function useUsuarioById(id: string | null) {
  return useQuery({
    queryKey: ['usuarios', id],
    queryFn: () => getUsuarioById(id!),
    enabled: !!id,
  });
}

export function usePerfil() {
  return useQuery({
    queryKey: ['perfil'],
    queryFn: () => getPerfil(),
  });
}

export function useRoles() {
  return useQuery({
    queryKey: ['roles'],
    queryFn: () => getRoles(),
    staleTime: 1000 * 60 * 60, // 1 hora — los roles no cambian frecuentemente
  });
}

export function useCreateUsuario() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateUsuarioRequest) => createUsuario(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['usuarios'] });
    },
  });
}

export function useUpdateUsuario() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateUsuarioRequest }) =>
      updateUsuario(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['usuarios'] });
    },
  });
}

export function useCambiarPassword() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      data,
    }: {
      id: string;
      data: CambiarPasswordRequest;
    }) => cambiarPassword(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['perfil'] });
    },
  });
}

export function useToggleActivo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, activar }: { id: string; activar: boolean }) =>
      toggleActivo(id, activar),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['usuarios'] });
    },
  });
}

export function useAsignarRol() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, rolId }: { id: string; rolId: string }) =>
      asignarRol(id, rolId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['usuarios'] });
    },
  });
}