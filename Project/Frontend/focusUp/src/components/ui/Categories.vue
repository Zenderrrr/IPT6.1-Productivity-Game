<script lang="ts" setup>
  const props = defineProps<{
    text: string,
    isActive: boolean,
  }>();

  const emit = defineEmits<{
    (e: 'clicked') : void,
  }>();

  function buttonClicked() {
    emit('clicked');
  }
</script>

<template>
  <div @click="buttonClicked" :class="props.isActive ? 'active' : 'inactive'" class="cursor-pointer text-center px-3 py-2 text-sm border text-nowrap rounded-full inline">
    <span>{{ props.text }}</span>
  </div>
</template>

<style scoped>
  .active{
    border-color: var(--primary-color);
    color: var(--primary-color);
    background-color: var(--primary-color-light);
  }

  .inactive{
    border-color: var(--color-gray-200);
    color: var(--text-color-light);
    background-color: var(--surface-color);
  }
</style>
