<template>
  <DataTable :value="list" :size="'large'" tableStyle="min-width: 50rem">
    <Column field="name" header="Nombre"></Column>
    <Column field="birthDate" header="BirthDate"></Column>
  </DataTable>
  <Button label="Add Accountant" icon="pi pi-plus" @click="addVisible = true" severity="success" />
  <RegisterAccountantDrawer v-if="isEmpty || addVisible" :visibility="addVisible" @accountantSaved="loadAccountingData" />
  <ModifyAccountantDrawer />
</template>

<script setup lang="ts">
  import { ref, onMounted, onBeforeMount } from 'vue';
  import type { Accounting } from '@/models/Accounting';
  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';
  import RegisterAccountantDrawer from './RegisterAccountantDrawer.vue';
  import axios from 'axios';
  import Button from 'primevue/button';
  import ModifyAccountantDrawer from './ModifyAccountantDrawer.vue';

  const isEmpty = ref<boolean>(true);
  const list = ref<Accounting[]>([]);
  const addVisible = ref<boolean>(false);
  const props = defineProps<{
    listAccountants?: Accounting[],
    visibility?: boolean,
    accountant?: Accounting
  }>();

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

  onBeforeMount(() => {
    loadAccountingData();
  });
</script>

