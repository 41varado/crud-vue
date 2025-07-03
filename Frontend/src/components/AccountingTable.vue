<template>
  <DataTable :value="list" :size="'large'" tableStyle="min-width: 50rem">
    <Column field="name" header="Nombre"></Column>
    <Column field="birthDate" header="BirthDate"></Column>
    <Column>
      <template #body="{ data }">
        <Button label="Modify" icon="pi pi-pencil" @click="openModifyDialog(data)" severity="warn" />
        <Button label="Delete" class="mx-3" icon="pi pi-trash" @click="openDeleteDialog(data)" severity="danger" />
      </template>
    </Column>
  </DataTable>
  <Button label="Add Accountant" icon="pi pi-plus" @click="addVisible = true" severity="success" />
  <RegisterAccountantDrawer v-if="isEmpty || addVisible" @accountantListModified="loadAccountingData" />
  <ModifyAccountantDrawer v-if="modifyVisible" @accountantListModified="loadAccountingData" @close="closeModifyDialog"
    :accountant="selectedAccountant" />
  <DeleteAccountantDrawer v-if="deleteVisible" @accountantListModified="loadAccountingData" @close="closeDeleteDialog"
    :accountant="selectedAccountant" />
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Accounting } from '@/models/Accounting';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import RegisterAccountantDrawer from './RegisterAccountantDrawer.vue';
import axios from 'axios';
import Button from 'primevue/button';
import ModifyAccountantDrawer from './ModifyAccountantDrawer.vue';
import DeleteAccountantDrawer from './DeleteAccountantDrawer.vue';

const isEmpty = ref<boolean>(true);
const list = ref<Accounting[]>([]);
const addVisible = ref<boolean>(false);
const modifyVisible = ref<boolean>(false);
const deleteVisible = ref<boolean>(false);
const selectedAccountant = ref<Accounting | null>(null);

const loadAccountingData = () => {
  axios.get<Accounting[]>('https://localhost:44349/api/v1/accounting')
    .then(response => {
      isEmpty.value = response.data.length === 0;
      list.value = response.data.map((item: Accounting) => {
        const date = new Date(item.birthDate);
        item.birthDate = date.toLocaleDateString('es-ES', {
          day: '2-digit',
          month: '2-digit',
          year: 'numeric',
        });
        return item;
      });
      console.log('Accounting data loaded successfully:', list.value);
    })
    .catch(error => {
      console.error('Error fetching accounting data:', error);
    });
}

const openModifyDialog = (accountant: Accounting) => {
  selectedAccountant.value = { ...accountant };
  modifyVisible.value = true;
}

const closeModifyDialog = () => {
  selectedAccountant.value = null;
  setInterval(() => {
    modifyVisible.value = false;
  }, 3000);
}

const openDeleteDialog = (accountant: Accounting) => {
  console.log('Opening delete dialog for:', accountant);
  selectedAccountant.value = null;
  setTimeout(() => {
    selectedAccountant.value = JSON.parse(JSON.stringify(accountant));
    deleteVisible.value = true;
  }, 0);
};

const closeDeleteDialog = () => {
  console.log('Closing delete dialog');
  selectedAccountant.value = null;
  setInterval(() => {
    deleteVisible.value = false;
  }, 3000);
};

onMounted(() => {
  loadAccountingData();
});
</script>
