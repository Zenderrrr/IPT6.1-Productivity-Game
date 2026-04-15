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
  <div @click="buttonClicked" :class="props.isActive ? 'active' : '' " class="hover:border-gray-400 transition duration-200 cursor-pointer text-center px-3 py-2 text-sm bg-[var(--surface-color)] text-[var(--text-color-light)] border border-gray-200 text-nowrap rounded-full inline">
    <span>{{ props.text }}</span>
  </div>
</template>

<style scoped>
  .active{
    border-color: var(--primary-color);
    color: var(--primary-color);
    background-color: var(--primary-color-light);
  }
</style>
