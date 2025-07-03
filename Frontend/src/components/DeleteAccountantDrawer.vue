<template>
  <Toast />
  <ConfirmDialog group="delete-accountant" :autoFocus="false">
    <template #message="slotProps">
        <div class="flex flex-col items-center w-full gap-4 border-b border-surface-200 dark:border-surface-700">
            <i :class="slotProps.message.icon" class="!text-6xl text-primary-500"></i>
            <p>{{ slotProps.message.message }}</p>
        </div>
    </template>
  </ConfirmDialog>
</template>

<script setup lang="ts">
  import type { Accounting } from '@/models/Accounting';
  import { ref, watch } from 'vue';
  import axios from 'axios';
  import Toast from 'primevue/toast';
  import { useToast } from 'primevue/usetoast';
  import ConfirmDialog from 'primevue/confirmdialog';
  import { useConfirm } from "primevue/useconfirm";

  const props = defineProps<{
    accountant: Accounting | null;
  }>();
  const accountantToLoad = ref<Accounting | null>(null);
  const dialogVisible = ref<boolean>(props.accountant !== null);
  const toast = useToast();
  const confirm = useConfirm();
  
  const emit = defineEmits(['accountantListModified', 'close']);

  watch(
  () => props.accountant,
  (newAccountant) => {
    console.log('Accountant to delete:', newAccountant);
    accountantToLoad.value = newAccountant ? { ...newAccountant } : null;
    if (newAccountant) {
      confirm.require({
        group: 'delete-accountant',
        message: `Are you sure you want to delete ${newAccountant.name}?`,
        icon: 'pi pi-exclamation-triangle',
        acceptLabel: 'Yes',
        rejectLabel: 'No',
        acceptIcon: 'pi pi-check',
        rejectIcon: 'pi pi-times',
        acceptClass: 'p-button-danger',
        rejectClass: 'p-button-secondary',
        accept: () => {
          console.log('Si')
          deleteAccountant();
        },
        reject: () => {
          handleDialogClose();
        }
      });
    }
  }
  );

  const handleDialogClose = () => {
    accountantToLoad.value = null;
    dialogVisible.value = false;
    emit('close');
  };

  const deleteAccountant = () => {
    console.log(accountantToLoad.value);
    if (accountantToLoad.value?.id) {
      const name = accountantToLoad.value.name;
      console.log(name);
      axios.delete(`https://localhost:44349/api/v1/accounting/${name}`)
        .then(() => {
          toast.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Accountant deleted successfully!',
            life: 3000
          });
          emit('accountantListModified');
        })
        .catch((error) => {
          console.error('Error deleting accountant:', error);
          toast.add({
            severity: 'error',
            summary: 'Error',
            detail: error.message,
            life: 3000
          });
        });
    }
    handleDialogClose();
  };
</script>